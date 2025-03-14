<script setup lang="ts">
import { ref, onMounted } from 'vue';
import axios from 'axios';
import { defineComponent } from "vue";
import Navbar from "./components/Navbar.vue";
import Feed from "./components/Feed.vue";
import Footer from "./components/Footer.vue";

interface User {
    id: number;
    name: string;
}

const users = ref<User[]>([]);
const newUser = ref('');
const errorMessage = ref('');
const successMessage = ref('');

const fetchUsers = async () => {
    try {
        const response = await axios.get('/api/users');
        users.value = response.data;
        errorMessage.value = '';
    } catch (error) {
        console.error("API Error:", error);
        errorMessage.value = "Failed to load users.";
    }
};

const addUser = async () => {
    if (!newUser.value.trim()) {
        errorMessage.value = "Please enter a valid name.";
        return;
    }

    try {
        const response = await axios.post('/api/users', { name: newUser.value });

        if (response.status === 201) { 
            successMessage.value = "User added successfully!";
            errorMessage.value = '';
            newUser.value = '';
            fetchUsers();
        }
    } catch (error) {
        console.error("API Error:", error);
        errorMessage.value = "Failed to add user.";
    }
};

onMounted(fetchUsers);
</script>
<template>
      <div>
        <h1>Users</h1>

        <p v-if="errorMessage" style="color: red">{{ errorMessage }}</p>

        <p v-if="successMessage" style="color: green">{{ successMessage }}</p>

        <ul>
            <li v-for="user in users" :key="user.id">{{ user.name }}</li>
        </ul>

        <input v-model="newUser" placeholder="Enter name" />
        <button @click="addUser">Add User</button>
    </div>
  <div id="app">
    <Navbar />
    <main>
      <Feed />
    </main>
    <Footer />
  </div>
</template>

<script lang="ts">
export default defineComponent({
  name: "App",
  components: {
    Navbar,
    Feed,
    Footer,
  },
});
</script>

<style>
/* Global styles */
:root {
  --background: #1e1e1e;
  --text-color: #f4f4f4;
  --primary-color: #4a90e2;
  --secondary-color: #ff4081;
}

body {
  font-family: "Inter", sans-serif;
  background-color: var(--background);
  color: var(--text-color);
  margin: 0;
  padding: 0;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
}

#app {
  width: 90%;
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem;
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
}

main {
  width: 100%;
  flex: 1;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 2rem;
}

.navbar, .footer {
  width: 100%;
  max-width: 1200px;
  padding: 1rem;
  text-align: center;
}

.card {
  width: 100%;
  max-width: 600px;
  padding: 1.5rem;
  background: rgba(255, 255, 255, 0.1);
  border-radius: 12px;
  text-align: left;
}
</style>

  
