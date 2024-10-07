<!-- src/components/Chat.vue -->
<template>
  <div class="container mt-5">
    <div class="card">
      <div class="card-header bg-primary text-white">
        Chat 
      </div>
      <div class="card-body" style="height: 400px; overflow-y: auto;">
        <div v-for="(message, index) in messages" :key="index" class="mb-2">
          <strong>{{ message.username }}:</strong> {{ message.text }}
        </div>
      </div>
      <div class="card-footer">
        <form @submit.prevent="sendMessage">
          <div class="input-group">
            <input
              type="text"
              class="form-control"
              v-model="newMessage"
              placeholder="Type a message"
              required
            />
            <button class="btn btn-primary" type="submit">Send</button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount } from 'vue'
import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr'

interface Message {
  username: string
  text: string
}

const newMessage = ref('')
const messages = ref<Message[]>([])
const username = ref('')

let connection: HubConnection

const sendMessage = () => {
  if (newMessage.value.trim() !== '') {
    const message: Message = {
      username: username.value || 'Anonymous',
      text: newMessage.value
    }
    connection.invoke('SendMessage', message)
    newMessage.value = ''
  }
}

onMounted(async () => {
  username.value = prompt('Enter your username:') || 'Anonymous'

  connection = new HubConnectionBuilder()
    .withUrl('http://localhost:5235/chat') // server side url
    .build()

  connection.on('message', (message: Message) => {    
    messages.value.push(message);
  })

  await connection.start()
})

onBeforeUnmount(() => {
  connection.stop()
})
</script>

<style scoped>
.card-body {
  background-color: #f8f9fa;
  border-radius: 5px;
  padding: 15px;
}
</style>