<template>
    <div class="feed">
      <div v-for="post in posts" :key="post.id" class="post-card">
        <div class="post-header">
          <img class="avatar" :src="post.avatar" alt="User Avatar" />
          <span class="username">{{ post.username }}</span>
        </div>
        <img class="post-image" :src="post.image" alt="Post Image" />
        <div class="post-footer">
          <p class="caption">{{ post.caption }}</p>
        </div>
      </div>
    </div>
  </template>
  
  <script lang="ts">
  import { defineComponent, ref } from "vue";
  
  interface Post {
    id: number;
    username: string;
    avatar: string;
    image: string;
    caption: string;
  }
  
  export default defineComponent({
    name: "Feed",
    setup() {
      // Dummy posts; replace with actual API calls or state management
      const posts = ref<Post[]>([
        {
          id: 1,
          username: "user1",
          avatar: "https://via.placeholder.com/50",
          image: "https://via.placeholder.com/600",
          caption: "Loving this new day!",
        },
        {
          id: 2,
          username: "user2",
          avatar: "https://via.placeholder.com/50",
          image: "https://via.placeholder.com/600",
          caption: "Exploring the campus.",
        },
        {
          id: 3,
          username: "user3",
          avatar: "https://via.placeholder.com/50",
          image: "https://via.placeholder.com/600",
          caption: "A moment to remember.",
        },
        // Add more posts as needed
      ]);
  
      return { posts };
    },
  });
  </script>
  
  <style scoped>
  .feed {
    display: flex;
    flex-direction: column;
    gap: 20px;
    padding: 20px 0;
  }
  .post-card {
    border: 1px solid #ddd;
    border-radius: 8px;
    overflow: hidden;
    background-color: #fff;
    max-width: 600px;
    margin: 0 auto;
  }
  .post-header {
    display: flex;
    align-items: center;
    padding: 10px;
  }
  .avatar {
    border-radius: 50%;
    width: 40px;
    height: 40px;
    margin-right: 10px;
  }
  .username {
    font-weight: bold;
    color: #333;
  }
  .post-image {
    width: 100%;
    object-fit: cover;
  }
  .post-footer {
    padding: 10px;
    text-align: left;
  }
  .caption {
    margin: 0;
    color: #555;
  }
  </style>
  