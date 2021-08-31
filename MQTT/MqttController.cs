using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using System.Text.Json;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt;
using HortaIoT.Models;
using HortaIoT.Repository;
using Microsoft.Extensions.Configuration;

namespace HortaIoT.Controllers
{
    [ApiController]
    [Route("mqtt")]
    public class MqttController : ControllerBase
    {
        MqttClient client;
        private IDataRepository _dataRepository;
        private string[] subscribeTopics = {"data"};
        private byte[] qosLevels = {MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE};
        private string clientId = "Horta"+ Guid.NewGuid().ToString();

        public MqttController(MqttClient mqttClient, IDataRepository dataRepository, IConfiguration configuration){
            _dataRepository = dataRepository;
            client = mqttClient;
            
            client.MqttMsgPublishReceived += messageReceived;
            
            client.Connect(clientId, configuration["MqttBroker:User"], configuration["MqttBroker:Password"]);

            client.Subscribe(subscribeTopics, qosLevels);
            
        }

        private void messageReceived(object sender, MqttMsgPublishEventArgs e){
            if(e.Topic == "data"){
                DataModel data = JsonSerializer.Deserialize<DataModel>(e.Message);
                _dataRepository.Add(data);
            }
        }

        [HttpGet("motor/{state}")]
        public IActionResult motor(string state){
            int intState = state == "on"? 1 : 0;

            client.Publish("motor", Encoding.UTF8.GetBytes(intState.ToString()));
            return Ok();
        }

        [HttpGet("led/{level}/{state}")]
        public IActionResult led(int level, string state){
            int intState = state == "on"? 1 : 0;
            int ledState = level -1 + intState;
            
            client.Publish("led", Encoding.UTF8.GetBytes(ledState.ToString()));
            return Ok();
        }

        [HttpGet("start")]
        public IActionResult start(){
            //endpoint to force the application to instantiate the controller
            return Ok("Mqtt service started");
        }
           
    }

}