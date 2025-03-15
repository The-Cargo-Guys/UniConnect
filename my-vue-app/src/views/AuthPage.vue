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
  
  <script setup>
  import { ref } from "vue";
  import axios from "axios";
  import { useRouter } from "vue-router";
  
  const router = useRouter();
  const isLogin = ref(true);
  const email = ref("");
  const password = ref("");
  const name = ref("");
  const phoneNumber = ref("");
  const loading = ref(false);
  const errorMessage = ref("");
  
  const toggleForm = () => {
    isLogin.value = !isLogin.value;
    errorMessage.value = "";
  };
  
  // Login Function
  const login = async () => {
    loading.value = true;
    errorMessage.value = "";

    try {
        const response = await axios.post("https://localhost:7004/api/auth/login", {
            email: email.value,
            password: password.value,
        });

        if (response.data.token) {
            localStorage.setItem("token", response.data.token);
            router.push("/");

            // 🔄 Force a full page refresh to ensure proper authentication state
            setTimeout(() => {
                window.location.reload();
            }, 500);
        } else {
            throw new Error("Invalid response from server");
        }
    } catch (error) {
        errorMessage.value = error.response?.data?.message || "Invalid email or password";
    } finally {
        loading.value = false;
    }
};

  
  // Register Function
  const register = async () => {
    loading.value = true;
    errorMessage.value = "";
  
    try {
      await axios.post("https://localhost:7004/api/auth/register", {
        name: name.value,
        email: email.value,
        password: password.value,
        phoneNumber: phoneNumber.value,
      });
  
      alert("Registration successful! Please log in.");
      isLogin.value = true;
    } catch (error) {
      errorMessage.value = error.response?.data?.message || "Registration failed";
    } finally {
      loading.value = false;
    }
  };
  </script>
  
  <style scoped>
/* Center the authentication page */
.auth-page {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  background: linear-gradient(to right, #007bff, #00c6ff);
}

/* Auth container (login/register box) */
.auth-container {
  width: 100%;
  max-width: 400px;
  padding: 25px;
  background: white;
  border-radius: 10px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
  text-align: center;
  animation: fadeIn 0.5s ease-in-out;
}

/* Smooth fade-in animation */
@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(-20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

/* Headings */
.auth-container h2 {
  margin-bottom: 15px;
  font-size: 24px;
  color: #333;
}

/* Form styles */
form {
  display: flex;
  flex-direction: column;
}

/* Input fields */
input {
  width: 100%;
  padding: 12px;
  margin: 10px 0;
  border: 1px solid #ccc;
  border-radius: 8px;
  font-size: 16px;
  transition: border-color 0.3s ease-in-out;
}

/* Input focus effect */
input:focus {
  border-color: #007bff;
  outline: none;
  box-shadow: 0 0 5px rgba(0, 123, 255, 0.5);
}

/* Button styles */
button {
  width: 100%;
  padding: 12px;
  font-size: 18px;
  background: #007bff;
  color: white;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  transition: background 0.3s ease-in-out, transform 0.2s;
}

/* Button hover and active effects */
button:hover {
  background: #0056b3;
}

button:active {
  transform: scale(0.98);
}

/* Disabled button */
button:disabled {
  background: #aaa;
  cursor: not-allowed;
}

/* Error message */
.error-message {
  color: red;
  font-size: 14px;
  margin-top: 10px;
}

/* Switch between login/register */
p {
  margin-top: 15px;
  font-size: 14px;
  color: #555;
}

a {
  color: #007bff;
  text-decoration: none;
  font-weight: bold;
  cursor: pointer;
  transition: color 0.3s ease-in-out;
}

a:hover {
  color: #0056b3;
}

/* Mobile responsiveness */
@media (max-width: 500px) {
  .auth-container {
    width: 90%;
    padding: 20px;
  }

  h2 {
    font-size: 22px;
  }
}
</style>
