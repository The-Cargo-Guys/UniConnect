<template>
    <v-container class="fill-height d-flex justify-center align-center">
      <v-card class="profile-card">
        <!-- Profile Header -->
        <v-avatar size="100" class="profile-avatar">
          <v-img v-if="user?.imagePath" :src="user.imagePath" alt="User Profile" />
          <v-img v-else :src="'/pp-fallback.png'" alt="Profile Picture"></v-img>

        </v-avatar>
  
        <v-card-title class="profile-name">{{ user?.name }}</v-card-title>
        <v-card-subtitle class="profile-email">{{ user?.email }}</v-card-subtitle>
  
        <v-divider></v-divider>
  
        <!-- Profile Details -->
        <v-card-text>
          <v-row>
            <v-col cols="12" md="6">
              <p><v-icon icon="mdi-phone" class="icon" /> {{ user?.phoneNumber || "N/A" }}</p>
              <p><v-icon icon="mdi-school" class="icon" /> {{ user?.university || "Not specified" }}</p>
              <p><v-icon icon="mdi-certificate" class="icon" /> {{ user?.degree || "Not specified" }}</p>
            </v-col>
            <v-col cols="12" md="6">
              <p><v-icon icon="mdi-information-outline" class="icon" /> {{ user?.bio || "No bio provided" }}</p>
              <p><v-icon icon="mdi-account-check-outline" class="icon" />
                {{ user?.isAdmin ? "Administrator" : "Regular User" }}</p>
            </v-col>
          </v-row>
  
          <!-- Tags Section -->
          <div v-if="user?.tags?.length" class="tags-container">
            <v-chip v-for="(tag, index) in user.tags" :key="index" class="tag-chip" color="light-blue">
              {{ tag.value }}
            </v-chip>
          </div>
  
          <v-btn color="light-blue darken-2" class="back-btn" @click="goBack">
            <v-icon left>mdi-arrow-left</v-icon> Back
          </v-btn>
        </v-card-text>
      </v-card>
    </v-container>
  </template>
  
  <script setup lang="ts">
  import { ref, onMounted, computed } from "vue";
  import { useRouter, useRoute } from "vue-router";
  import axios from "axios";
  
  // Define interfaces for API response
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
  
  onMounted(fetchUserDetails);
  </script>
  
  <style scoped>
  /* Overall Container */
  .fill-height {
    background-color: white;
    color: white;
    padding: 40px;
  }
  
  /* Profile Card */
  .profile-card {
    max-width: 600px;
    width: 100%;
    background: #434361;
    color: white;
    padding: 30px;
    border-radius: 15px;
    box-shadow: 0px 4px 10px rgba(100, 30, 100, 0.5);
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
    color: lightgray;
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
    background-color: lightblue;
    color: lightblue;
  }
  
  /* Back Button */
  .back-btn {
    margin-top: 20px;
    width: 100%;
    font-weight: bold;
  }
  </style>
  