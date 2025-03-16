<script setup lang="ts">
import { ref } from "vue";

var menuItems = ref([
	{ title: "Home", path: "/", icon: "mdi-home" },
  { title: "Connect", path: "/connect", icon: "mdi-account-search" },
  { title: "Societies", path: "/societies", icon: "mdi-basketball" },
	{ title: "Courses", path: "/courses", icon: "mdi-account-multiple" },
]);
</script>
<template>
  <div class="wrapper">
    <v-bottom-navigation
      grow
      class="bottom-nav"
      :elevation="24"
      mode="shift"
      rounded="xl"
    >
      <v-spacer></v-spacer>
      <v-btn v-for="item in menuItems" :key="item.title" :to="item.path">
        <v-icon>{{ item.icon }}</v-icon>
        <span>{{ item.title }}</span>
      </v-btn>
      <v-spacer></v-spacer>
    </v-bottom-navigation>
  </div>
</template>

<style scoped>
.wrapper {
  position: relative;
  display: flex;
  justify-content: center;
}

.bottom-nav {
  margin-bottom: 10px;
  width: 800px !important;
  left: auto !important;
  right: auto !important;
  z-index: 1000;
}
</style>
