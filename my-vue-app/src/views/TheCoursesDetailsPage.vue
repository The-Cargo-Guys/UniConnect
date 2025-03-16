<template>
    <div class="society-details">
      <v-card color="secondary">
        <h1>{{ course.name }}</h1>
      </v-card>
      <v-card>
        <v-card-title class="text-left">DESCRIPTION</v-card-title>
        <v-card-subtitle class="text-left" :style="{ opacity: '100%' }">{{ course.description }}</v-card-subtitle>
        <v-tabs v-model="activeTab" background-color="primary" dark>
          <v-tab>Feed</v-tab>
          <v-tab>Members</v-tab>
        </v-tabs>
        <v-tabs-window v-model="activeTab">
          <v-tabs-window-item>
            <v-card v-for="post in feed" :key="post.id" class="mb-2" outlined>
              <v-card-text>{{ post.content }}</v-card-text>
            </v-card>          </v-tabs-window-item>
          <v-tabs-window-item>
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
          </v-tabs-window-item>
        </v-tabs-window>
      </v-card>
    </div>
  </template>
  
  <script lang="ts">
  import { defineComponent, ref, onMounted } from 'vue';
  import { useRoute } from 'vue-router';
  
  export default defineComponent({
    name: "SocietyDetailsPage",
    setup() {
      const tabs = ["Feed", "Members"];
      const activeTab = ref(0);

      const route = useRoute();
      const courseId = route.params.id as string;
  
      // Replace these dummy values with real data fetches
      const course = ref({
        id: courseId,
        name: "Course " + courseId,
        description: "Description for course " + courseId,
      });


      const feed = ref([
        { id: 1, content: "Post 1 for society " + courseId },
        { id: 2, content: "Post 2 for society " + courseId },
      ]);
      const members = ref([
        { id: 1, name: "Member 1", role: "President" },
        { id: 2, name: "Member 2", role: "Vice President" },
      ]);
  
      onMounted(() => {
        console.log("Loaded details for society", courseId);
      });
  
      return {
        tabs,
        course,
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
  