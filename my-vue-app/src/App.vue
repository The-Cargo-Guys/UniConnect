<script setup lang="ts">
import { useAuth0 } from '@auth0/auth0-vue';
import { useRouter } from 'vue-router';
import { RouterView } from 'vue-router';
import TheNavBar from './components/TheNavBar.vue';
import AuthPage from './views/AuthPage.vue';
import { computed, watchEffect, ref, onMounted, watch } from 'vue';

const { loginWithRedirect, logout, isAuthenticated, isLoading } = useAuth0();
const router = useRouter();
const isLoggedIn = ref(false);

// 🚀 **Check authentication on app load BEFORE rendering**
onMounted(() => {
    const token = localStorage.getItem("token");

    if (token) {
        console.log("✅ Found stored token, setting isLoggedIn = true");
        isLoggedIn.value = true;
    } else {
        console.log("❌ No token found, redirecting to /auth");
        isLoggedIn.value = false;
        router.push("/auth");
    }
});

// 🔄 **Watch localStorage for instant updates**
watch(() => localStorage.getItem("token"), (newToken) => {
    if (newToken) {
        console.log("🔄 Token updated, setting isLoggedIn = true");
        isLoggedIn.value = true;
        router.push("/");
    } else {
        console.log("❌ Token removed, redirecting to /auth");
        isLoggedIn.value = false;
        router.push("/auth");
    }
});

// 🔄 **Watch Auth0 authentication changes dynamically**
watchEffect(() => {
    console.log("🔄 Auth state changed:", isAuthenticated.value);

    if (isAuthenticated.value) {
        localStorage.setItem("token", "auth0-user"); // Simulate storing token
        isLoggedIn.value = true;
        router.push("/");
    } else if (!localStorage.getItem("token")) {
        localStorage.removeItem("token");
        isLoggedIn.value = false;
        router.push("/auth");
    }
});

// ✅ **Handle Logout - Clears storage & cookies**
const handleLogout = () => {
    // Clear all local storage, session storage, and cookies
    localStorage.clear();
    sessionStorage.clear();

    document.cookie.split(";").forEach((c) => {
        document.cookie = c
            .replace(/^ +/, "")
            .replace(/=.*/, `=;expires=${new Date(0).toUTCString()};path=/`);
    });

    isLoggedIn.value = false; // Update UI state
};

</script>


<template>
  <v-app class="fade-in">
    <!-- ⏳ Show loading state -->
    <template v-if="isLoading">
      <div class="loading-screen">Loading...</div>
    </template>

    <!-- 🔑 Show login/register page if user is NOT authenticated -->
    <template v-else-if="!isLoggedIn">
      <AuthPage />
    </template>

    <!-- 🏠 Show main content when the user IS authenticated -->
    <template v-else>
      <TheNavBar />

      <v-app-bar app color="primary" dark>
        <v-toolbar-title class="d-flex align-center">
          <v-img 
            src="/path/to/your/logo.png" 
            alt="Logo" 
            contain 
            max-height="40" 
            max-width="40">
          </v-img>
          <span class="ml-3 font-weight-bold">UniConnect</span>
        </v-toolbar-title>
        <v-spacer></v-spacer>

        <v-btn text @click="handleLogout">Log Out</v-btn>
      </v-app-bar>

      <v-main>
        <RouterView />
      </v-main>
    </template>
  </v-app>
</template>

<style scoped>
.loading-screen {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  font-size: 20px;
  font-weight: bold;
}
</style>
