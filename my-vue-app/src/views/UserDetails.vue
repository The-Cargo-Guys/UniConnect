<template>
  <v-app>
    <v-main class="profile-container">
      <v-container>
        <!-- Profile Card -->
        <v-card class="profile-card" elevation="4">
          <v-card-title class="text-h5 font-weight-bold">User Profile</v-card-title>

          <v-divider></v-divider>

          <!-- Loading State -->
          <v-row v-if="loading" justify="center">
            <v-progress-circular indeterminate color="light-blue darken-1"></v-progress-circular>
          </v-row>

          <!-- Error State -->
          <v-alert v-else-if="error" type="error" class="error-message">
            {{ error }}
          </v-alert>

          <!-- User Details -->
          <v-card-text v-else-if="user">
            <v-row justify="center">
              <!-- Profile Image -->
              <v-avatar size="120">
                <v-img :src="user.imagePath || 'https://via.placeholder.com/120'" alt="User Profile"></v-img>
              </v-avatar>
            </v-row>

            <v-list dense class="profile-details">
              <v-list-item>
                <v-list-item-title><strong>Name:</strong> {{ user.name }}</v-list-item-title>
              </v-list-item>
              <v-list-item>
                <v-list-item-title><strong>Email:</strong> {{ user.email }}</v-list-item-title>
              </v-list-item>
              <v-list-item>
                <v-list-item-title><strong>Phone:</strong> {{ user.phoneNumber || "Not provided" }}</v-list-item-title>
              </v-list-item>
              <v-list-item>
                <v-list-item-title><strong>Bio:</strong> {{ user.bio || "No bio available" }}</v-list-item-title>
              </v-list-item>
              <v-list-item>
                <v-list-item-title><strong>University:</strong> {{ user.university || "Not specified" }}</v-list-item-title>
              </v-list-item>
              <v-list-item>
                <v-list-item-title><strong>Degree:</strong> {{ user.degree || "Not specified" }}</v-list-item-title>
              </v-list-item>
              <v-list-item>
                <v-list-item-title><strong>Admin Status:</strong> {{ user.isAdmin ? "Admin" : "Regular User" }}</v-list-item-title>
              </v-list-item>

              <!-- Tags Section -->
              <v-list-item v-if="user.tags && user.tags.length > 0">
                <v-list-item-title>
                  <strong>Tags:</strong>
                  <v-chip v-for="(tag, index) in user.tags" :key="index" class="tag-chip">
                    {{ tag.name }}
                  </v-chip>
                </v-list-item-title>
              </v-list-item>
              <v-list-item v-else>
                <v-list-item-title>No tags added</v-list-item-title>
              </v-list-item>
            </v-list>
          </v-card-text>

          <v-divider></v-divider>

          <!-- Buttons -->
          <v-card-actions class="button-container">
            <v-btn class="modern-btn edit-btn" @click="goToEditUserPage">
              Edit Info
            </v-btn>
            <v-btn class="modern-btn back-btn" @click="goBack">
              Back
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-container>
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';

const user = ref<any>(null);
const loading = ref(true);
const error = ref<string | null>(null);
const router = useRouter();

const fetchUserData = async () => {
  const userId = localStorage.getItem("userId");

  if (!userId || userId === "null" || userId === "undefined") {
    console.log("❌ No userId found, redirecting to /auth");
    router.push("/auth");
    return;
  }

  try {
    console.log(`🔍 Fetching user data for userId: ${userId}`);
    const response = await fetch(`/api/users/${encodeURIComponent(userId)}`);

    if (!response.ok) {
      throw new Error("Failed to fetch user data");
    }

    user.value = await response.json();
  } catch (err) {
    error.value = "Error loading user details.";
  } finally {
    loading.value = false;
  }
};

onMounted(fetchUserData);

const goToEditUserPage = () => {
  router.push('/edit-user');
};

const goBack = () => {
  router.push('/');
};
</script>

<style scoped>
/* Main Container */
.profile-container {
  background: white;
  min-height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
}

/* Profile Card */
.profile-card {
  max-width: 500px;
  margin: auto;
  padding: 20px;
  text-align: center;
  border-radius: 12px;
  box-shadow: 0px 6px 15px rgba(0, 0, 0, 0.1);
}

/* User Details List */
.profile-details {
  margin-top: 15px;
}

/* Tag Chips */
.tag-chip {
  margin: 5px;
  background: linear-gradient(135deg, #007BFF, #00C6FF);
  color: white;
  font-size: 14px;
  font-weight: bold;
  border-radius: 20px;
  padding: 6px 12px;
}

/* Buttons */
.button-container {
  display: flex;
  justify-content: space-between;
  padding-top: 10px;
}

/* Edit & Back Buttons */
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

.edit-btn {
  background: linear-gradient(135deg, #28A745, #00C851);
}

.edit-btn:hover {
  background: linear-gradient(135deg, #218838, #007F3D);
}

.back-btn {
  background: linear-gradient(135deg, #FF4444, #CC0000);
}

.back-btn:hover {
  background: linear-gradient(135deg, #D32F2F, #B71C1C);
}

/* Error Message */
.error-message {
  text-align: center;
  font-size: 16px;
  color: red;
  margin-top: 10px;
}
</style>
