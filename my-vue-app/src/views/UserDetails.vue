<template>
    <div class="user-details-container">
      <h2>User Details</h2>
  
      <div v-if="loading">Loading user data...</div>
      <div v-else-if="error">{{ error }}</div>
      <div v-else-if="user">
        <!-- ✅ Display Profile Image if Available -->
        <img v-if="user.imagePath" :src="user.imagePath" alt="User Profile" class="profile-image" />
  
        <p><strong>Name:</strong> {{ user.name }}</p>
        <p><strong>Email:</strong> {{ user.email }}</p>
        <p><strong>Phone:</strong> {{ user.phoneNumber }}</p>
        <p><strong>Bio:</strong> {{ user.bio || "No bio provided" }}</p>
        <p><strong>University:</strong> {{ user.university || "Not specified" }}</p>
        <p><strong>Degree:</strong> {{ user.degree || "Not specified" }}</p>
  
        <!-- ✅ Display Tags -->
        <p v-if="user.tags && user.tags.length > 0">
          <strong>Tags:</strong>
          <span v-for="(tag, index) in user.tags" :key="index" class="tag">
            {{ tag.name }}
          </span>
        </p>
        <p v-else>No tags added</p>
  
        <p><strong>Admin Status:</strong> {{ user.isAdmin ? "Admin" : "Regular User" }}</p>
  
        <button @click="goBack">Back</button>
      </div>
    </div>
  </template>
  
  <script>
  import { ref, onMounted } from "vue";
  import { useRouter } from "vue-router";
  
  export default {
    setup() {
      const user = ref(null);
      const loading = ref(true);
      const error = ref(null);
      const router = useRouter();
  
      const fetchUserData = async () => {
        const userId = localStorage.getItem("userId");
  
        // ✅ Check if userId is valid
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
  
      const goBack = () => {
        router.push("/");
      };
  
      return { user, loading, error, goBack };
    },
  };
  </script>
  
  <style scoped>
  .user-details-container {
    max-width: 500px;
    margin: auto;
    text-align: center;
    display: flex;
    flex-direction: column;
    gap: 15px;
  }
  
  .profile-image {
    width: 120px;
    height: 120px;
    border-radius: 50%;
    object-fit: cover;
    margin: 10px auto;
  }
  
  .tag {
    display: inline-block;
    background-color: #007bff;
    color: white;
    padding: 5px 10px;
    border-radius: 15px;
    margin: 2px;
    font-size: 0.9em;
  }
  </style>
  