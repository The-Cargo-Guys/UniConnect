<script setup lang="ts">
import { useAuth0 } from '@auth0/auth0-vue';
import { HelloWorld } from '../components';
import { ref, onMounted } from 'vue'

const username = ref("")
const oldUsername = ref("");
const password = ref("")
const firstName = ref("")
const lastName = ref("")
const email = ref("")
const deletePassword = ref("")
const confirmDelete = ref(false)

const showPassword = ref(false)
const errorMessages = ref<string[]>([]); // Array to handle multiple errors
const successMessage = ref("");

</script>

<template>
    <div>
      <div class="LoginContainer">
        <div class="LoginBox">
          <h1>Edit Information</h1>
          <div class="ItemGroups">
            <label>Username</label>
            <input v-model="username" type="text" />
          </div>
          <div class="ItemGroups">
            <label>Password</label>
            <div class="PasswordInput">
              <input v-model="password" :type="showPassword ? 'text' : 'password'" />
            </div>
            <label>Show Password</label>
            <div class="ShowPassword">
                <input v-model="showPassword" type="checkbox" style="padding:0%" />
            </div>
          </div>
          <div class="ItemGroups">
            <label>First Name</label>
            <input v-model="firstName" type="text" />
          </div>
          <div class="ItemGroups">
            <label>Last Name</label>
            <input v-model="lastName" type="text" />
          </div>
          <div class="ItemGroups">
            <label>Email</label>
            <input v-model="email" type="text" />
          </div>
  
          <!-- Buttons for Save Changes and Delete Account -->
          <div class="ButtonGroup">
            <button>Save Changes</button>
          </div>
  
          <div class="Spacing"></div>
          
          <div class="DeleteAccount">
            <h2>Delete Account</h2>
            <div class="ItemGroups">
              <label>Account Password</label>
              <input v-model="deletePassword" type="password" placeholder="Enter your password" />
            </div>
            <div class="ItemGroups">
              <input v-model="confirmDelete" type="checkbox" />
              <label for="confirmDelete">Confirm</label>
            </div>
            <button class="delete-button">Delete Account</button>
          </div>
  
          <div class="ErrorMessage">
            <ul>
              <li v-for="(error, index) in errorMessages" :key="index">{{ error }}</li>
            </ul>
          </div>
          <div class="SuccessMessage">
            {{ successMessage }}
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <style scoped>
  .LoginContainer {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh; /* Ensure the container fills the whole height */
  background-color: #f0f0f0;
  padding: 10px;
  box-sizing: border-box;
  overflow-y: auto; /* Allow vertical scrolling */
}

.LoginBox {
  display: flex;
  flex-direction: column;
  align-items: center;
  width: 100%;
  max-width: 500px;
  gap: 15px; /* Increase gap for spacing */
  padding: 20px;
  border: 1px solid #ccc;
  border-radius: 8px;
  background-color: #fff;
  box-sizing: border-box;
}

.LoginBox h1 {
  margin-bottom: 20px;
  text-align: center;
}

.ItemGroups {
  text-align: center;
  margin-bottom: 12px;
  width: 100%;
}

.ItemGroups label {
  display: block;
  margin-bottom: 5px;
}

.ItemGroups input {
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
  width: 100%; /* Make the input fill the container */
  max-width: 400px; /* Max width for inputs */
}

.PasswordInput {
  width: 100%;
}

.ShowPassword {
  margin-left: 10px;
}

.ButtonGroup {
  margin-bottom: 20px;
  width: 100%;
  display: flex;
  justify-content: center;
}

button {
  padding: 10px 20px;
  border: none;
  border-radius: 4px;
  background-color: #007bff;
  color: white;
  cursor: pointer;
  width: 100%; /* Ensure button fits within the container */
  max-width: 400px; /* Limit button width */
}

button:hover {
  background-color: #0056b3;
}

.delete-button {
  background-color: #dc3545;
}

.delete-button:hover {
  background-color: #c82333;
}

.ErrorMessage {
  margin-top: 5px;
  color: red;
  font-size: 12px;
}

.SuccessMessage {
  margin-top: 5px;
  color: green;
  font-size: 12px;
}

.DeleteAccount {
  text-align: center;
  margin-top: 10px;
  width: 100%;
}

.DeleteAccount h2 {
  font-size: 18px;
  font-weight: bold;
  color: #dc3545;
}

.Spacing {
  height: 20px;
}

@media (max-width: 600px) {
  .LoginBox {
    padding: 15px;
    max-width: 90%; /* Allow more space on smaller screens */
  }

  .ItemGroups input,
  .ButtonGroup button {
    max-width: 100%;
  }

  .DeleteAccount h2 {
    font-size: 16px;
  }
}

  </style>