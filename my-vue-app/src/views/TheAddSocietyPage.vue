<template>
    <v-app>
      <v-main class="add-society-container">
        <v-container class="d-flex justify-center align-center fill-height">
          <v-card class="society-card" elevation="8">
            <!-- Title -->
            <v-card-title class="text-h5 font-weight-bold">
              Create a New Society
            </v-card-title>
  
            <v-divider></v-divider>
  
            <!-- Society Creation Form -->
            <v-card-text>
              <v-form @submit.prevent="submitSociety">
                <v-row>
                  <v-col cols="12">
                    <v-text-field v-model="name" label="Society Name" variant="outlined" prepend-inner-icon="mdi-account-group" required></v-text-field>
                  </v-col>
  
                  <v-col cols="12">
                    <v-textarea v-model="description" label="Description" variant="outlined" prepend-inner-icon="mdi-text-box-outline" required></v-textarea>
                  </v-col>
  
                  <v-col cols="12">
                    <v-text-field v-model="imagePathBanner" label="Banner Image URL" variant="outlined" prepend-inner-icon="mdi-image" required></v-text-field>
                  </v-col>
                </v-row>
              </v-form>
            </v-card-text>
  
            <!-- Submit Button -->
            <v-card-actions class="button-container">
              <v-btn class="modern-btn create-btn" block @click="submitSociety">
                <v-icon left>mdi-plus-box</v-icon> Create Society
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-container>
      </v-main>
    </v-app>
  </template>
  
  <script setup lang="ts">
import { ref } from "vue";
import { useRouter } from "vue-router";
import axios from "axios";
import { Society } from "../apiClient";

const router = useRouter();
const name = ref("");
const description = ref("");
const imagePathBanner = ref("");

const submitSociety = async () => {
  try {
    const society = new Society({
      name: name.value,
      description: description.value,
      imagePathBanner: imagePathBanner.value,
    });

    const response = await axios.post("api/societies/create", society);
    alert(`Society Created: ${response.data.name}`);
    router.push("/societies");
  } catch (error) {
    console.error("Error creating society:", error);
    alert("Failed to create society. Please check the server logs for details.");
  }
};
</script>

<style scoped>
/* Main Container */
.add-society-container {
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

/* Society Creation Card */
.society-card {
  max-width: 600px;
  width: 100%;
  background: white;
  color: black;
  padding: 30px;
  border-radius: 15px;
  box-shadow: 0px 4px 12px rgba(0, 0, 0, 0.1);
  text-align: center;
}

/* Input Fields */
.v-text-field,
.v-textarea {
  margin-bottom: 15px;
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

/* Create Button */
.create-btn {
  background: linear-gradient(135deg, #28A745, #00C851);
}

.create-btn:hover {
  background: linear-gradient(135deg, #218838, #007F3D);
}
</style>
