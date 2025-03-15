<template>
    <div class="auth-container">
      <h2>Register</h2>
      <form @submit.prevent="register">
        <input v-model="name" type="text" placeholder="Full Name" required />
        <input v-model="email" type="email" placeholder="Email" required />
        <input v-model="password" type="password" placeholder="Password" required />
        <input v-model="phoneNumber" type="text" placeholder="Phone Number" required />
        <button type="submit" :disabled="loading">{{ loading ? "Registering..." : "Register" }}</button>
        <p v-if="errorMessage" class="error-message">{{ errorMessage }}</p>
      </form>
      <p>Already have an account? <router-link to="/login">Login here</router-link></p>
    </div>
  </template>
  
  <script>
  import axios from "axios";
  
  export default {
    data() {
      return {
        name: "",
        email: "",
        password: "",
        phoneNumber: "",
        loading: false,
        errorMessage: "",
      };
    },
    methods: {
      async register() {
        this.loading = true;
        this.errorMessage = "";
  
        try {
          await axios.post("/api/auth/register", {
            name: this.name,
            email: this.email,
            password: this.password,
            phoneNumber: this.phoneNumber,
          });
  
          // Redirect to login page after successful registration
          this.$router.push("/login");
        } catch (error) {
          this.errorMessage = error.response?.data || "Registration failed";
        } finally {
          this.loading = false;
        }
      },
    },
  };
  </script>
  
  <style scoped>
  .auth-container {
    max-width: 400px;
    margin: 50px auto;
    padding: 20px;
    border: 1px solid #ccc;
    border-radius: 5px;
    background: #fff;
    text-align: center;
  }
  
  input {
    width: 90%;
    padding: 10px;
    margin: 10px 0;
    border: 1px solid #ccc;
    border-radius: 5px;
  }
  
  button {
    width: 100%;
    padding: 10px;
    background: #28a745;
    color: white;
    border: none;
    cursor: pointer;
  }
  
  button:disabled {
    background: #aaa;
  }
  
  .error-message {
    color: red;
  }
  </style>
  