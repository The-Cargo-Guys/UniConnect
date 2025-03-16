<template>
    <div>
      <button class="edit-user" type="button" @click="goToEditUserPage" aria-label="Edit User Info">
        Edit Info
      </button>
    </div>
  </template>
  
  <script setup lang="ts">
  import { useRouter } from 'vue-router';
  
  const router = useRouter();
  
  const goToEditUserPage = () => {
    router.push('/edit-user');
  };
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
  