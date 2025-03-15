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
            <v-card v-for="post in feed" :key="post.id" class="mb-2" outlined>
              <v-card-text>{{ post.content }}</v-card-text>
            </v-card>
          </div>
        </v-tab-item>
        <v-tab-item>
          <div class="members-list">
            <v-list two-line>
              <v-list-item v-for="member in members" :key="member.id">
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
  
      // Replace these dummy values with real data fetches
      const society = ref({
        id: societyId,
        name: "Society " + societyId,
        description: "Description for society " + societyId,
      });
      const feed = ref([
        { id: 1, content: "Post 1 for society " + societyId },
        { id: 2, content: "Post 2 for society " + societyId },
      ]);
      const members = ref([
        { id: 1, name: "Member 1", role: "President" },
        { id: 2, name: "Member 2", role: "Vice President" },
      ]);
      const activeTab = ref(0);
  
      onMounted(() => {
        console.log("Loaded details for society", societyId);
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
  .feed-wall, .members-list {
    margin-top: 1rem;
  }
  </style>
  