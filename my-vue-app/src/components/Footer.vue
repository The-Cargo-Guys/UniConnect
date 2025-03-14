<template>
    <footer class="footer">
      <p>&copy; 2025 UniConnect. All rights reserved.</p>
    </footer>
  </template>
  
  <script lang="ts">
  import { defineComponent } from "vue";
  export default defineComponent({
    name: "Footer",
  });
  </script>
  
  <style scoped>
  .footer {
    text-align: center;
    padding: 20px;
    border-top: 1px solid #ddd;
    margin-top: 20px;
    color: #777;
  }
  </style>
  