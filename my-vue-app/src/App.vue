<script setup lang="ts">
  import { TheNavBar } from './components';
</script>

<template>
  <v-app class="fade-in">
    <the-nav-bar></the-nav-bar>
    <v-main>
      <RouterView></RouterView>
    </v-main>
  </v-app>
</template>

<style scoped>
.fade-in {
    animation: fadeIn 0.4s ease-in;
}

@keyframes fadeIn {
    from {
        opacity: 0;
    }
    to {
        opacity: 1;
    }
}
</style>
