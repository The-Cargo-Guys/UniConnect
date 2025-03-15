<script setup lang="ts">
import { useAuth0 } from '@auth0/auth0-vue';
import { RouterView } from 'vue-router';
import { TheNavBar } from './components';
import AuthPage from "../views/AuthPage.vue"; 

const { loginWithRedirect, isAuthenticated, logout, user } = useAuth0();
</script>

<template>
  <v-app class="fade-in">
    <!-- Show login/register page if user is NOT authenticated -->
    <template v-if="!isAuthenticated">
      <AuthPage />
    </template>

    <!-- Show main content when the user IS authenticated -->
    <template v-else>
      <the-nav-bar></the-nav-bar>
      
      <!-- Global Navbar -->
      <v-app-bar app color="primary" dark>
        <!-- Logo and Title -->
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

        <!-- Authentication Buttons -->
        <v-btn text disabled>Welcome, {{ user?.email }}</v-btn>
        <v-btn text @click="logout">Log Out</v-btn>
      </v-app-bar>
      
      <!-- Main Content -->
      <v-main>
        <RouterView />
      </v-main>
    </template>
  </v-app>
</template>

<style scoped>
.fade-in {
  animation: fadeIn 0.4s ease-in;
}

@keyframes fadeIn {
  from { opacity: 0; }
  to { opacity: 1; }
}

.ml-3 {
  margin-left: 1rem;
}
</style>
