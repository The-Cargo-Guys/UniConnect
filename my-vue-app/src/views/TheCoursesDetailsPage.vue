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
            <TheCoursePostCarousel></TheCoursePostCarousel>
          </v-tabs-window-item>
          <v-tabs-window-item>
          </v-tabs-window-item>
        </v-tabs-window>
      </v-card>
    </div>
  </template>

  <script setup lang="ts">
    import { ref, onMounted } from 'vue';
    import { useRoute } from 'vue-router';
    import axios from 'axios';
    import { TheCoursePostCarousel } from "../components";
    const activeTab = ref(0);

    const route = useRoute();
    const courseId = route.params.id as string;

    const course = ref({
        id: courseId,
        name: "Course " + courseId,
        description: "Description for course " + courseId,
      });

      const loading = ref(false);
      const error = ref('');

      const fetchCourse = async () => {
      loading.value = true;
      try {
        const response = await axios.get(`/get-by-id/{${courseId}`);
        course.value = response.data;
      } catch (err) {
        console.error('API Error:', err);
        error.value = 'Failed to load societies.';
      } finally {
        loading.value = false;
      }
    };

    onMounted(() => {
        fetchCourse();
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
  