<template>
    <div class="society-details">
      <v-card color="secondary">
        <h1>{{ society?.name }}</h1>
      </v-card>
      <v-card>
        <v-card-title class="text-left">DESCRIPTION</v-card-title>
        <v-card-subtitle class="text-left" :style="{ opacity: '100%' }">{{ society?.description }}</v-card-subtitle>
        <v-tabs v-model="activeTab" background-color="primary" dark>
          <v-tab>Feed</v-tab>
          <v-tab>Members</v-tab>
        </v-tabs>
        <v-tabs-window v-model="activeTab">
          <v-tabs-window-item>
            <TheSocietyPostCarousel></TheSocietyPostCarousel>
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
    import { TheSocietyPostCarousel } from "../components";

    const route = useRoute();
    const societyId = route.params.id as string;
    const activeTab = ref(0);

    interface Society {
      id: number;
      name: string;
      description: string;
      banner: string;
    }

    const society = ref<Society>();
    const loading = ref(false);
    const error = ref('');

    const fetchSociety = async () => {
      loading.value = true;
      try {
        const response = await axios.get(`/get-society/${societyId}`);
        society.value = response.data;
      } catch (err) {
        console.error('API Error:', err);
        error.value = 'Failed to load societies.';
      } finally {
        loading.value = false;
      }
    };

    onMounted(() => {
      fetchSociety()
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
  