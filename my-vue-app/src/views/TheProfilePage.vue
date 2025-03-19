<template>
  <v-app>
    <v-main class="profile-container">
      <v-container class="d-flex justify-center align-center fill-height">
        <!-- Profile Card -->
        <v-card class="profile-card" elevation="8">
          <!-- Profile Header -->
          <v-avatar size="120" class="profile-avatar">
            <v-img v-if="user?.imagePath" :src="user.imagePath" alt="User Profile" />
            <v-img v-else src="/pp-fallback.png" alt="Profile Picture" />
          </v-avatar>

          <v-card-title class="profile-name">{{ user?.name }}</v-card-title>
          <v-card-subtitle class="profile-email">{{ user?.email }}</v-card-subtitle>

          <v-divider></v-divider>

          <!-- Profile Details -->
          <v-card-text>
            <v-row>
              <v-col cols="12" md="6">
                <p><v-icon class="icon">mdi-phone</v-icon> {{ user?.phoneNumber || "N/A" }}</p>
                <p><v-icon class="icon">mdi-school</v-icon> {{ user?.university || "Not specified" }}</p>
                <p><v-icon class="icon">mdi-certificate</v-icon> {{ user?.degree || "Not specified" }}</p>
              </v-col>
              <v-col cols="12" md="6">
                <p><v-icon class="icon">mdi-information-outline</v-icon> {{ user?.bio || "No bio provided" }}</p>
                <p><v-icon class="icon">mdi-account-check-outline</v-icon>
                  {{ user?.isAdmin ? "Administrator" : "Regular User" }}
                </p>
              </v-col>
            </v-row>

            <!-- Tags Section -->
            <div v-if="user?.tags?.length" class="tags-container">
              <v-chip v-for="(tag, index) in user.tags" :key="index" class="tag-chip">
                {{ tag.value }}
              </v-chip>
            </div>
          </v-card-text>

          <v-divider></v-divider>

          <!-- Buttons -->
          <v-card-actions class="button-container">
            <v-btn class="modern-btn edit-btn" @click="goToEditUserPage">
              <v-icon left>mdi-pencil</v-icon> Edit Info
            </v-btn>
            <v-btn class="modern-btn back-btn" @click="goBack">
              <v-icon left>mdi-arrow-left</v-icon> Back
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-container>
    </v-main>
  </v-app>
</template>


<script setup lang="ts">
import { ref, onMounted, computed } from "vue";
import { useRouter, useRoute } from "vue-router";
import axios from "axios";

// Define interfaces
interface Tag {
  id: string;
  value: string;
}

interface UserProfile {
  id: string;
  name: string;
  email: string;
  phoneNumber: string;
  bio?: string;
  university: string;
  degree: string;
  imagePath?: string;
  tags?: Tag[];
  isAdmin: boolean;
}

const route = useRoute();
const router = useRouter();
const userId = computed(() => route.params.id as string | undefined);

const user = ref<UserProfile | null>(null);
const error = ref<string | null>(null);
const loading = ref<boolean>(true);

const fetchUserDetails = async () => {
  if (!userId.value) {
    error.value = "Invalid user ID.";
    loading.value = false;
    return;
  }

  try {
    console.log(`🔍 Fetching user data for ID: ${userId.value}`);
    const response = await axios.get<UserProfile>(`/api/users/${encodeURIComponent(userId.value)}`);
    console.log("✅ User data received:", response.data);
    user.value = response.data;
  } catch (err: any) {
    console.error("❌ Error fetching user data:", err);
    error.value = err.response?.data?.message || "Failed to load user details.";
  } finally {
    loading.value = false;
  }
};

const goBack = () => {
  router.push("/");
};

const goToEditUserPage = () => {
  router.push("/edit-user");
};

onMounted(fetchUserDetails);
</script>

<style scoped>
/* Main Container (Full Screen Centering) */
.profile-container {
  background: white;
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
}

/* Ensures Centering Inside Vuetify */
.fill-height {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
}

/* Profile Card */
.profile-card {
  max-width: 600px;
  width: 100%;
  background: white;
  color: black;
  padding: 30px;
  border-radius: 15px;
  box-shadow: 0px 4px 12px rgba(0, 0, 0, 0.1);
  text-align: center;
}

/* Avatar Styling */
.profile-avatar {
  margin: 0 auto;
  border: 3px solid lightblue;
}

/* Profile Name & Email */
.profile-name {
  font-size: 24px;
  font-weight: bold;
  margin-top: 10px;
}

.profile-email {
  color: gray;
  font-size: 16px;
}

/* Icons */
.icon {
  color: lightblue;
  margin-right: 8px;
}

/* Tags */
.tags-container {
  margin-top: 15px;
}

.tag-chip {
  margin: 5px;
  font-size: 14px;
  background: linear-gradient(135deg, #007BFF, #00C6FF);
  color: white;
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
