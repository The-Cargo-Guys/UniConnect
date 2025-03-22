<script setup lang="ts">
import { ref, computed } from "vue";
import { useRouter, useRoute } from "vue-router";

const router = useRouter();
const route = useRoute();
const emit = defineEmits(["logout"]);
const drawer = ref(false);

// Navigation links
const menuItems = [
  { title: "My Societies", route: "/societies" },
  { title: "My Courses", route: "/courses" },
];

// Toggle drawer (sidebar)
const toggleDrawer = () => {
  drawer.value = !drawer.value;
};

// Determine if the current page is a society or course page (for add button)
const showAddButton = computed(() => route.path.includes("/societies") || route.path.includes("/courses"));
</script>

<template>
  <v-app-bar color="white" flat fixed class="nav">
    <!-- Hamburger Menu -->
    <v-app-bar-nav-icon @click="toggleDrawer" color="black"></v-app-bar-nav-icon>

    <!-- Logo -->
    <v-avatar class="logo-wrapper">
      <v-img src="/UniConnect.svg" alt="Logo" contain></v-img>
    </v-avatar>

    <v-toolbar-title class="black--text">UniConnect</v-toolbar-title>

    <v-spacer></v-spacer>

    <!-- Profile & Logout Buttons -->
    <v-btn to="/profile" flat>
      <v-icon>mdi-account</v-icon>
      <span>Profile</span>
    </v-btn>
    <v-btn @click="$emit('logout')" flat>Log Out</v-btn>
  </v-app-bar>

  <!-- Sidebar Navigation Drawer -->
  <v-navigation-drawer v-model="drawer" temporary app color="#f5f5f5">
    <v-list>
      <v-list-item v-for="item in menuItems" :key="item.route" @click="router.push(item.route)">
        <v-list-item-content>
          <v-list-item-title class="black--text">{{ item.title }}</v-list-item-title>
        </v-list-item-content>
      </v-list-item>
    </v-list>
  </v-navigation-drawer>
</template>

<style scoped>
.nav {
  position: fixed;
  top: 0;
  width: 100%;
  z-index: 1000;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
}

.logo-wrapper {
  height: 50px;
  width: 50px;
  margin-left: 10px;
}
</style>
