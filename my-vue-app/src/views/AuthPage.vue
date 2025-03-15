<template>
    <div class="auth-page">
      <div class="auth-container">
        <h2 v-if="isLogin">Login</h2>
        <h2 v-else>Register</h2>
  
        <form @submit.prevent="isLogin ? login() : register()">
          <input v-if="!isLogin" v-model="name" type="text" placeholder="Full Name" required />
          <input v-model="email" type="email" placeholder="Email" required />
          <input v-model="password" type="password" placeholder="Password" required />
          <input v-if="!isLogin" v-model="phoneNumber" type="text" placeholder="Phone Number" required />
          
          <button type="submit" :disabled="loading">
            {{ loading ? (isLogin ? "Logging in..." : "Registering...") : (isLogin ? "Login" : "Register") }}
          </button>
          
          <p v-if="errorMessage" class="error-message">{{ errorMessage }}</p>
        </form>
  
        <p>
          {{ isLogin ? "Don't have an account?" : "Already have an account?" }}
          <a @click="toggleForm">{{ isLogin ? "Register here" : "Login here" }}</a>
        </p>
      </div>
    </div>
  </template>
  
  <script>
  import axios from "axios";
  
  export default {
    data() {
      return {
        isLogin: true,
        name: "",
        email: "",
        password: "",
        phoneNumber: "",
        loading: false,
        errorMessage: "",
      };
    },
    methods: {
      toggleForm() {
        this.isLogin = !this.isLogin;
        this.errorMessage = "";
      },
      async login() {
        this.loading = true;
        this.errorMessage = "";
  
        try {
          const response = await axios.post("/api/auth/login", {
            email: this.email,
            password: this.password,
          });
  
          localStorage.setItem("token", response.data.token);
          this.$router.push("/dashboard");
        } catch (error) {
          this.errorMessage = error.response?.data || "Invalid email or password";
        } finally {
          this.loading = false;
        }
      },
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
  .auth-page {
    display: flex;
    justify-content: center;
    align-items: center;
    height: 100vh;
    background: #f4f4f4;
  }
  .auth-container {
    max-width: 400px;
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
    background: #007bff;
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
  a {
    color: #007bff;
    cursor: pointer;
  }
  </style>
  