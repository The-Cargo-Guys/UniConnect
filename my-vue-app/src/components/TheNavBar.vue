<script setup lang="ts">
import { ref } from "vue";

var menuItems = ref([
  { title: "Home", path: "/", icon: "mdi-home" },
  { title: "Societies", path: "/societies", icon: "mdi-basketball" },
  { title: "Communities", path: "/communities", icon: "mdi-account-multiple" },
  { title: "Profile", path: "/profile", icon: "mdi-account" },
  { title: "Settings", path: "/settings", icon: "mdi-cog" },
]);
</script>

<template>
  <v-bottom-navigation elevation="12">
    <v-spacer></v-spacer>
      <v-btn v-for="item in menuItems" :key="item.title" :to="item.path">
        <v-icon>{{ item.icon }}</v-icon>
        <p>{{ item.title }}</p>
      </v-btn>
    <v-spacer></v-spacer>
  </v-bottom-navigation>
</template>
