<script setup lang="ts">
import { ref, onMounted } from "vue";
import axios from "axios";
import { User } from "../apiClient";

const users = ref<User[]>([]);
const newUser = ref("");
const errorMessage = ref("");
const successMessage = ref("");

const fetchUsers = async () => {
  try {
    const response = await axios.get("/api/users");
    users.value = response.data;
    errorMessage.value = "";
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
    const response = await axios.post("/api/users", { name: newUser.value });

    if (response.status === 201) {
      successMessage.value = "User added successfully!";
      errorMessage.value = "";
      newUser.value = "";
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
      <p>{{ users.length }}</p>
      <input v-model="newUser" placeholder="Enter name" />
      <button @click="addUser">Add User</button>

      <ul>
        <li v-for="user in users" :key="user.id">{{ user.name }}</li>
      </ul>

    </div>
</template>

<style scoped>
.read-the-docs {
  color: #888;
}
</style>
