<template>
  <nav class="navbar">
    <div class="navbar-container">
      <div class="logo">UniConnect</div>
      <ul class="nav-links">
        <li><a href="/">Home</a></li>
        <li><a href="/explore">Explore</a></li>
        <li><a href="/notifications">Notifications</a></li>
        <li><a href="/profile">Profile</a></li>
      </ul>
    </div>
  </nav>
</template>

<script lang="ts">
import { defineComponent } from "vue";
export default defineComponent({
  name: "Navbar",
});
</script>

<style scoped>
/* Make sure the navbar spans full width and stays on top */
.navbar {
  position: sticky;
  top: 0;
  z-index: 100;
  width: 100%;
  background-color: #ffffff;
  border-bottom: 1px solid #ddd;
}

/* Limit content width and center it */
.navbar-container {
  display: flex;
  align-items: center;
  justify-content: space-between;
  max-width: 1200px; /* Adjust as needed */
  margin: 0 auto;
  padding: 10px 20px;
}

/* Branding */
.logo {
  font-size: 1.5em;
  font-weight: bold;
  color: #333;
}

/* Navigation links */
.nav-links {
  list-style: none;
  display: flex;
  gap: 20px;
  margin: 0;
  padding: 0;
}
.nav-links li a {
  text-decoration: none;
  color: #333;
  font-weight: 500;
}
.nav-links li a:hover {
  color: #646cff;
}
</style>
