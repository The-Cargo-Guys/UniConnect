<template>
  <v-app>
    <v-main class="edit-container">
      <v-container class="d-flex justify-center align-center fill-height">
        <v-card class="edit-card" elevation="8">
          <v-card-title class="text-h5 font-weight-bold">Edit Information</v-card-title>

          <v-divider></v-divider>

          <!-- Edit Form -->
          <v-card-text>
            <v-row>
              <v-col cols="12">
                <v-text-field v-model="username" label="Username" variant="outlined" prepend-inner-icon="mdi-account"></v-text-field>
              </v-col>

              <v-col cols="12">
                <v-text-field
                  v-model="password"
                  :type="showPassword ? 'text' : 'password'"
                  label="Password"
                  variant="outlined"
                  prepend-inner-icon="mdi-lock"
                  :append-inner-icon="showPassword ? 'mdi-eye-off' : 'mdi-eye'"
                  @click:append-inner="showPassword = !showPassword"
                ></v-text-field>
              </v-col>

              <v-col cols="12" md="6">
                <v-text-field v-model="firstName" label="First Name" variant="outlined" prepend-inner-icon="mdi-account-outline"></v-text-field>
              </v-col>

              <v-col cols="12" md="6">
                <v-text-field v-model="lastName" label="Last Name" variant="outlined" prepend-inner-icon="mdi-account-outline"></v-text-field>
              </v-col>

              <v-col cols="12">
                <v-text-field v-model="email" label="Email" variant="outlined" prepend-inner-icon="mdi-email"></v-text-field>
              </v-col>
            </v-row>
          </v-card-text>

          <!-- Buttons -->
          <v-card-actions class="button-container">
            <v-btn class="modern-btn save-btn" block>
              <v-icon left>mdi-content-save</v-icon> Save Changes
            </v-btn>
          </v-card-actions>

          <v-divider></v-divider>

          <!-- Delete Account Section -->
          <v-card-text class="delete-section">
            <h3>Delete Account</h3>

            <v-text-field
              v-model="deletePassword"
              label="Account Password"
              type="password"
              variant="outlined"
              prepend-inner-icon="mdi-lock"
            ></v-text-field>

            <v-checkbox v-model="confirmDelete" label="Confirm Deletion"></v-checkbox>

            <v-btn class="modern-btn delete-btn" block>
              <v-icon left>mdi-delete</v-icon> Delete Account
            </v-btn>
          </v-card-text>

          <!-- Error & Success Messages -->
          <v-alert v-if="errorMessages.length" type="error" class="error-message" dense>
            <ul>
              <li v-for="(error, index) in errorMessages" :key="index">{{ error }}</li>
            </ul>
          </v-alert>

          <v-alert v-if="successMessage" type="success" class="success-message" dense>
            {{ successMessage }}
          </v-alert>
        </v-card>
      </v-container>
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
import { ref } from 'vue';

const username = ref("");
const password = ref("");
const firstName = ref("");
const lastName = ref("");
const email = ref("");
const deletePassword = ref("");
const confirmDelete = ref(false);

const showPassword = ref(false);
const errorMessages = ref<string[]>([]);
const successMessage = ref("");
</script>

<style scoped>
/* Main Container */
.edit-container {
  background: white;
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 20px;
}

/* Fullscreen Centering */
.fill-height {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
}

/* Edit Card */
.edit-card {
  max-width: 600px;
  width: 100%;
  background: white;
  color: black;
  padding: 30px;
  border-radius: 15px;
  box-shadow: 0px 4px 12px rgba(0, 0, 0, 0.1);
  text-align: center;
}

/* Password Visibility Toggle */
.v-input__control {
  transition: box-shadow 0.3s;
}

.v-input__control:hover {
  box-shadow: 0px 0px 8px rgba(0, 123, 255, 0.3);
}

/* Buttons */
.button-container {
  display: flex;
  justify-content: center;
  padding-top: 10px;
}

/* Modern Buttons */
.modern-btn {
  background: linear-gradient(135deg, #007BFF, #00C6FF);
  color: white;
  font-size: 16px;
  font-weight: bold;
  text-transform: uppercase;
  padding: 10px 20px;
  border-radius: 30px;
  transition: all 0.3s ease-in-out;
}

.modern-btn:hover {
  transform: scale(1.05);
}

/* Save Button */
.save-btn {
  background: linear-gradient(135deg, #28A745, #00C851);
}

.save-btn:hover {
  background: linear-gradient(135deg, #218838, #007F3D);
}

/* Delete Account Section */
.delete-section {
  text-align: center;
  margin-top: 20px;
}

.delete-section h3 {
  font-size: 18px;
  font-weight: bold;
  color: #d32f2f;
}

/* Delete Button */
.delete-btn {
  background: linear-gradient(135deg, #FF4444, #CC0000);
}

.delete-btn:hover {
  background: linear-gradient(135deg, #D32F2F, #B71C1C);
}

/* Error Message */
.error-message {
  text-align: center;
  font-size: 16px;
  color: red;
  margin-top: 10px;
}

/* Success Message */
.success-message {
  text-align: center;
  font-size: 16px;
  color: green;
  margin-top: 10px;
}

/* Vuetify Components */
.v-text-field {
  margin-bottom: 15px;
}
</style>

