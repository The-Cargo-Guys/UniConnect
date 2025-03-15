<template>
    <div class="society-details">
      <h1>{{ society.name }}</h1>
      <p>{{ society.description }}</p>
  
      <v-tabs v-model="activeTab" background-color="primary" dark>
        <v-tab>Feed</v-tab>
        <v-tab>Members</v-tab>
      </v-tabs>
      <v-tabs-items v-model="activeTab">
        <v-tab-item>
          <div class="feed-wall">
            <v-card
              v-for="post in feed"
              :key="post.id"
              class="mb-2"
              outlined
            >
              <v-card-text>{{ post.content }}</v-card-text>
            </v-card>
          </div>
        </v-tab-item>
        <v-tab-item>
          <div class="members-list">
            <v-list two-line>
              <v-list-item
                v-for="member in members"
                :key="member.id"
              >
                <v-list-item-avatar>
                  <v-img src="https://via.placeholder.com/50"></v-img>
                </v-list-item-avatar>
                <v-list-item-content>
                  <v-list-item-title>{{ member.name }}</v-list-item-title>
                  <v-list-item-subtitle>{{ member.role || 'Member' }}</v-list-item-subtitle>
                </v-list-item-content>
              </v-list-item>
            </v-list>
          </div>
        </v-tab-item>
      </v-tabs-items>
    </div>
  </template>
  
  <script lang="ts">
  import { defineComponent, ref, onMounted } from 'vue';
  import { useRoute } from 'vue-router';
  
  export default defineComponent({
    name: "SocietyDetailsPage",
    setup() {
      const route = useRoute();
      const societyId = route.params.id as string;
  
      // Dummy data for demonstration. valin can link this to DB of users that have joined.
      const society = ref({
        id: societyId,
        name: "Society Name " + societyId,
        description: "This is a description for society " + societyId,
      });
  
      const feed = ref([
        { id: 1, content: "Post 1 from society " + societyId },
        { id: 2, content: "Post 2 from society " + societyId },
        { id: 3, content: "Post 3 from society " + societyId },
      ]);
  
      const members = ref([
        { id: 1, name: "Member 1", role: "Admin" },
        { id: 2, name: "Member 2", role: "Moderator" },
        { id: 3, name: "Member 3", role: "Member" },
      ]);
  
      const activeTab = ref(0);
  
      onMounted(() => {
        console.log("Loaded society details for ID:", societyId);
        // valin can fetch society details here based on societyId
      });
  
      return {
        society,
        feed,
        members,
        activeTab,
      };
    }
  });
  </script>
  
  <style scoped>
  .society-details {
    max-width: 800px;
    margin: 0 auto;
    padding: 2rem;
  }
  .feed-wall,
  .members-list {
    margin-top: 1rem;
  }
  </style>
  