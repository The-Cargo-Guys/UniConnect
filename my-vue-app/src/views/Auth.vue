<template>
  <div class="auth-container">
    <h2>{{ isLogin ? "Login" : "Register" }}</h2>

    <input v-if="!isLogin" v-model="name" placeholder="Full Name" type="text" />
    <input v-model="email" placeholder="Email" type="email" />
    <input v-model="password" placeholder="Password" type="password" />
    <input v-if="!isLogin" v-model="phoneNumber" placeholder="Phone Number" type="text" />

    <button @click="isLogin ? login() : register()">
      {{ isLogin ? "Login" : "Register" }}
    </button>

    <p>
      {{ isLogin ? "Don't have an account?" : "Already have an account?" }}
      <a href="#" @click="isLogin = !isLogin">
        {{ isLogin ? "Register" : "Login" }}
      </a>
    </p>
  </div>
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
          localStorage.setItem("userId", data.userId); // ✅ Store only the userId string
          console.log("✅ Login successful, userId set to:", data.userId);
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
          localStorage.setItem("userId", data.userId); // ✅ Store only the userId string
          console.log("✅ Registration successful, userId set to:", data.userId);
          router.push("/").then(() => window.location.reload());
        } else {
          console.error("❌ Registration response missing userId:", data);
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
.auth-container {
  max-width: 300px;
  margin: auto;
  display: flex;
  flex-direction: column;
  gap: 10px;
  text-align: center;
}
button {
  cursor: pointer;
}
</style>
