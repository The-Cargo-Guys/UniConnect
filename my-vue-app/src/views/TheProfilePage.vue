<template>
    <div>
      <div class="LoginContainer">
        <div class="LoginBox">
          <h1>My Details</h1>
          <div class="ItemGroups">
            <label>Profile Picture</label>
          </div>
          <div class="ItemGroups">
            <label>Username: </label> {{ user.name }}
          </div>
          <div class="ItemGroups">
            <label>First Name</label>
          </div>
          <div class="ItemGroups">
            <label>Last Name</label>
          </div>
          <div class="ItemGroups">
            <label>Email</label>
          </div>
          <div class="ItemGroups">
            <label>Phone Number</label>
          </div>
          <div class="ItemGroups">
            <label>Uni</label>
          </div>
          <div class="ItemGroups">
            <label>Degree</label>
          </div>
          <div class="ItemGroups">
            <label>Bio</label>
          </div>
        </div>
      </div>
    </div>
    <div>
      <button class="edit-user" type="button" @click="goToEditUserPage" aria-label="Edit User Info">
        Edit Info
      </button>
    </div>
  </template>
  
  <script setup lang="ts">
  import { useRouter } from 'vue-router';
  import { User } from "../apiClient";
  import { defineComponent, ref, onMounted } from 'vue';
  import axios from 'axios';
  
  const router = useRouter();
  
  const goToEditUserPage = () => {
    router.push('/edit-user');
  };

    const user = ref(User);
    const loading = ref(false);
    const error = ref('');

    const fetchUserInfo = async () => {
      loading.value = true;
      try {
        const response = await axios.get('/api/GetCurrentUser/');
        user.value = response.data;
      } catch (err) {
        console.error('API Error:', err);
        error.value = 'Failed to load user.';
      } finally {
        loading.value = false;
      }
    };
    
    onMounted(() => {
      fetchUserInfo();
    });
  
  </script>
  
  <style scoped>
  .edit-user {
    padding: 10px 20px;
    background-color: #007bff;
    color: white;
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
  </style>
  