<template>
  <v-container class="d-flex justify-center align-center" style="height: 100vh">
    <v-card class="auth-card px-6 py-8" elevation="10">
      <v-card-title class="text-center text-h5 font-weight-bold">
        {{ isLogin ? "Login" : "Register" }}
      </v-card-title>
      
      <v-card-text>
        <v-form @submit.prevent="isLogin ? login() : register()">
          <v-text-field
            v-if="!isLogin"
            v-model="name"
            label="Full Name"
            variant="outlined"
            color="blue-lighten-2"
          ></v-text-field>

          <v-text-field
            v-model="email"
            label="Email"
            variant="outlined"
            color="blue-lighten-2"
            type="email"
          ></v-text-field>

          <v-text-field
            v-model="password"
            label="Password"
            variant="outlined"
            color="blue-lighten-2"
            type="password"
          ></v-text-field>

          <v-text-field
            v-if="!isLogin"
            v-model="phoneNumber"
            label="Phone Number"
            variant="outlined"
            color="blue-lighten-2"
            type="text"
          ></v-text-field>

          <v-btn block color="blue-lighten-2" class="mt-3" type="submit">
            {{ isLogin ? "Login" : "Register" }}
          </v-btn>
        </v-form>

        <p class="mt-4 text-center text-body-2">
          {{ isLogin ? "Don't have an account?" : "Already have an account?" }}
          <a href="#" @click="isLogin = !isLogin" class="text-blue-lighten-2">
            {{ isLogin ? "Register" : "Login" }}
          </a>
        </p>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script>
import { ref } from "vue";
import { useRouter } from "vue-router";

export default {
  setup() {
    const isLogin = ref(true);
    const name = ref("");
    const email = ref("");
    const password = ref("");
    const phoneNumber = ref("");
    const router = useRouter();

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
        }
      } else {
        alert("Email already in use.");
      }
    };

    return { isLogin, name, email, password, phoneNumber, login, register };
  },
};
</script>

<style scoped>
.auth-card {
  max-width: 400px;
  background: #121212;
  color: white;
  border-radius: 12px;
  border: 1px solid rgba(173, 216, 230, 0.5);
}

.text-blue-lighten-2 {
  color: #81d4fa;
  cursor: pointer;
}
</style>
