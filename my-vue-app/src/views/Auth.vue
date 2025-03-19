<template>
  <v-app>
    <v-main class="auth-container">
      <v-container class="d-flex justify-center align-center fill-height">
        <v-card class="auth-card" elevation="8">
          <!-- Title -->
          <v-card-title class="text-h5 font-weight-bold">
            {{ isLogin ? "Login" : "Register" }}
          </v-card-title>

          <v-divider></v-divider>

          <!-- Authentication Form -->
          <v-card-text>
            <v-form>
              <v-row>
                <v-col cols="12" v-if="!isLogin">
                  <v-text-field v-model="name" label="Full Name" variant="outlined" prepend-inner-icon="mdi-account"></v-text-field>
                </v-col>

                <v-col cols="12">
                  <v-text-field v-model="email" label="Email" variant="outlined" prepend-inner-icon="mdi-email"></v-text-field>
                </v-col>

                <v-col cols="12">
                  <v-text-field
                    v-model="password"
                    :type="showPassword ? 'text' : 'password'"
                    label="Password"
                    variant="outlined"
                    prepend-inner-icon="mdi-lock"
                    :append-inner-icon="showPassword ? 'mdi-eye-off' : 'mdi-eye'"
                    @click:append-inner="showPassword = !showPassword"
                  ></v-text-field>
                </v-col>

                <v-col cols="12" v-if="!isLogin">
                  <v-text-field v-model="phoneNumber" label="Phone Number" variant="outlined" prepend-inner-icon="mdi-phone"></v-text-field>
                </v-col>
              </v-row>
            </v-form>
          </v-card-text>

          <!-- Buttons -->
          <v-card-actions class="button-container">
            <v-btn class="modern-btn action-btn" block @click="isLogin ? login() : register()">
              <v-icon left>mdi-login</v-icon> {{ isLogin ? "Login" : "Register" }}
            </v-btn>
          </v-card-actions>

          <v-divider></v-divider>

          <!-- Switch Login/Register -->
          <v-card-text class="switch-text">
            <p>
              {{ isLogin ? "Don't have an account?" : "Already have an account?" }}
              <a href="#" @click="toggleAuthMode">
                {{ isLogin ? "Register" : "Login" }}
              </a>
            </p>
          </v-card-text>
        </v-card>
      </v-container>
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useRouter } from "vue-router";

const isLogin = ref(true);
const name = ref("");
const email = ref("");
const password = ref("");
const phoneNumber = ref("");
const showPassword = ref(false);
const router = useRouter();

const toggleAuthMode = () => {
  isLogin.value = !isLogin.value;
};

const login = async () => {
  const response = await fetch("/api/auth/login", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email: email.value, password: password.value }),
  });

  const data = await response.json();
  if (response.ok) {
    if (data.userId) {
      localStorage.setItem("userId", data.userId);
      router.push("/").then(() => window.location.reload());
    } else {
      console.error("❌ Login response missing userId:", data);
    }
  } else {
    alert("Invalid credentials.");
  }
};

const register = async () => {
  const response = await fetch("/api/auth/register", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      name: name.value,
      email: email.value,
      password: password.value,
      phoneNumber: phoneNumber.value,
    }),
  });

  const data = await response.json();
  if (response.ok) {
    if (data.userId) {
      localStorage.setItem("userId", data.userId);
      router.push("/").then(() => window.location.reload());
    } else {
      console.error("❌ Registration response missing userId:", data);
    }
  } else {
    alert("Email already in use.");
  }
};
</script>

<style scoped>
/* Main Container */
.auth-container {
  background: white;
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 20px;
}

/* Fullscreen Centering */
.fill-height {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
}

/* Authentication Card */
.auth-card {
  max-width: 450px;
  width: 100%;
  background: white;
  color: black;
  padding: 30px;
  border-radius: 15px;
  box-shadow: 0px 4px 12px rgba(0, 0, 0, 0.1);
  text-align: center;
}

/* Input Fields */
.v-text-field {
  margin-bottom: 15px;
}

/* Buttons */
.button-container {
  display: flex;
  justify-content: center;
  padding-top: 10px;
}

/* Modern Buttons */
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

/* Switch Text */
.switch-text p {
  font-size: 14px;
  color: gray;
}

.switch-text a {
  color: #007BFF;
  font-weight: bold;
  text-decoration: none;
  cursor: pointer;
}

.switch-text a:hover {
  text-decoration: underline;
}
</style>
