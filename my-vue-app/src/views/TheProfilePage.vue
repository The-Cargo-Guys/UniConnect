<template>
    <div id="my-details-page" class="container">
      <h1>My Details</h1>
      <div class="user-info">
        <div class="item-group">
          <label>Profile Picture</label>
          <img v-if="user.profilePicture" :src="user.profilePicture" alt="Profile Picture" class="profile-img" />
        </div>
        <div class="item-group">
          <label>Username:</label> <span>{{ user.name }}</span>
        </div>
        <div class="item-group">
          <label>First Name:</label> <span>{{ user.firstName }}</span>
        </div>
        <div class="item-group">
          <label>Last Name:</label> <span>{{ user.lastName }}</span>
        </div>
        <div class="item-group">
          <label>Email:</label> <span>{{ user.email }}</span>
        </div>
        <div class="item-group">
          <label>Phone Number:</label> <span>{{ user.phoneNumber }}</span>
        </div>
        <div class="item-group">
          <label>Uni:</label> <span>{{ user.uni }}</span>
        </div>
        <div class="item-group">
          <label>Degree:</label> <span>{{ user.degree }}</span>
        </div>
        <div class="item-group">
          <label>Bio:</label> <span>{{ user.bio }}</span>
        </div>
      </div>
      <button class="edit-user" @click="goToEditUserPage" aria-label="Edit User Info">Edit Info</button>
      <div v-if="loading" class="loading">Loading your details...</div>
      <div v-if="error" class="error">{{ error }}</div>
    </div>
  </template>
  
  <script lang="ts">
  import { defineComponent, ref, onMounted } from 'vue';
  import { useRouter } from 'vue-router';
  import axios from 'axios';
  
  export default defineComponent({
    name: 'MyDetailsPage',
    setup() {
      const router = useRouter();
      interface User {
        name: string;
        firstName: string;
        lastName: string;
        email: string;
        phoneNumber: string;
        uni: string;
        degree: string;
        bio: string;
        profilePicture?: string;
      }
  
      const user = ref<User>({
        name: '',
        firstName: '',
        lastName: '',
        email: '',
        phoneNumber: '',
        uni: '',
        degree: '',
        bio: '',
      });
      const loading = ref(false);
      const error = ref('');
  
      const fetchUserInfo = async () => {
        loading.value = true;
        try {
          const response = await axios.get('/api/GetCurrentUser/');
          user.value = response.data;
        } catch (err) {
          console.error('API Error:', err);
          error.value = 'Failed to load user details.';
        } finally {
          loading.value = false;
        }
      };
  
      const goToEditUserPage = () => {
        router.push('/edit-user');
      };
  
      onMounted(() => {
        fetchUserInfo();
      });
  
      return {
        user,
        loading,
        error,
        goToEditUserPage,
      };
    },
  });
  </script>
  
  <style scoped>
  .container {
    max-width: 1200px;
    margin: auto;
    padding: 2rem;
    text-align: center;
  }
  
  h1 {
    font-size: 2rem;
    margin-bottom: 1rem;
  }
  
  .user-info {
    text-align: left;
    margin-bottom: 1.5rem;
  }
  
  .item-group {
    margin-bottom: 1rem;
  }
  
  .item-group label {
    font-weight: bold;
  }
  
  .profile-img {
    width: 100px;
    height: 100px;
    object-fit: cover;
    border-radius: 50%;
  }
  
  .edit-user {
    background-color: #4a90e2;
    color: white;
    padding: 10px 20px;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    font-size: 16px;
  }
  
  .edit-user:hover {
    background-color: #0056b3;
  }
  
  .edit-user:focus {
    outline: none;
    box-shadow: 0 0 5px 2px rgba(0, 123, 255, 0.6);
  }
  
  .loading {
    margin-top: 1rem;
    color: #4a90e2;
  }
  
  .error {
    margin-top: 1rem;
    color: red;
  }
  </style>
  