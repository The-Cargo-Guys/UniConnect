<template>
    <div class="dashboard-container">
      <h2>Welcome to Your Dashboard</h2>
      <p>You are logged in!</p>
      <button @click="logout">Logout</button>
    </div>
  </template>
  
  <script>
  export default {
    methods: {
      logout() {
        localStorage.removeItem("token"); // Remove JWT token
        this.$router.push("/login");
      },
    },
  };
  </script>
  
  <style scoped>
  .dashboard-container {
    text-align: center;
    margin-top: 50px;
  }
  
  button {
    padding: 10px;
    background: #dc3545;
    color: white;
    border: none;
    cursor: pointer;
  }
  </style>
  