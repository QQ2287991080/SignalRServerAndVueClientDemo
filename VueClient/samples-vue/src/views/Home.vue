<template>
  <div class="home">
    <h1>前端演示SignalR</h1>
    <input v-model="user" type="text" />
    <input v-model="message" type="text" />
    <button @click="sendAll">发送全部</button>
    <button @click="sendOwn">对自己发送</button>
    <button @click="sendClient">系统发送消息</button>

    <div>
      <ul v-for="(item ,index) in messages" v-bind:key="index +'itemMessage'">
        <li>{{item.user}} says {{item.message}}</li>
      </ul>
    </div>
  </div>
</template>

<script>
// @ is an alias to /src
import HelloWorld from "@/components/HelloWorld.vue";
import * as signalR from "@aspnet/signalr";
export default {
  name: "Home",
  components: {
    HelloWorld
  },
  data() {
    return {
      user: "", //用户
      message: "", //消息
      connection: "", //signalr连接
      messages: [] //返回消息
    };
  },
  methods: {
    //给全部发送消息
    sendAll: function() {
      this.connection
        .invoke("SendMessage", this.user, this.message)
        .catch(function(err) {
          return console.error(err);
        });
    },
    //只给自己发送消息
    sendOwn: function() {
      this.connection
        .invoke("SendMessageCaller", this.message)
        .catch(function(err) {
          return console.error(err);
        });
    },
    //系统发送消息
    sendClient: function() {
      this.$http.get("http://localhost:13989/api/test/get").then(resp => {
        console.log(resp);
      });
    }
  },
  created: function() {
    let thisVue = this;
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:13989/chathub", {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .configureLogging(signalR.LogLevel.Information)
      .build();
    this.connection.on("ReceiveMessage", function(user, message) {
      thisVue.messages.push({ user, message });
      console.log({ user, message });
    });
    this.connection.on("ReceiveCaller", function(message) {
      let user = "自己"; //这里为了push不报错，我就弄了一个默认值。
      thisVue.messages.push({ user, message });
      console.log({ user, message });
    });
    this.connection.start();
  }
};
</script>
