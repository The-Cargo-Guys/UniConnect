<template>
  <v-app>
    <v-navigation-drawer v-model="showDrawer" app color="white" light>
      <v-list>
        <v-list-item class="text-h6">My Societies</v-list-item>
        <v-divider></v-divider>
        <v-list-item v-for="society in societies" :key="society.id" @click="openSociety(society.id)">
          <v-list-item-content>
            <v-list-item-title>{{ society.name }}</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>

    <!-- Page Content -->
    <v-main class="content-container">
      <v-container>
        <!-- Add Society Button (Aligned with Cards) -->
        <v-row class="button-container">
          <v-col cols="12" md="6" lg="4">
            <v-btn class="modern-btn" @click="goToAddSociety">
              + Add Society
            </v-btn>
          </v-col>
        </v-row>

        <v-row>
          <v-col v-for="society in societies" :key="society.id" cols="12" md="6" lg="4">
            <v-card class="society-card" @click="openSociety(society.id)">
              <v-img :src="society.banner" class="society-banner"></v-img>
              <v-card-title>{{ society.name }}</v-card-title>
              <v-card-subtitle class="grey--text">{{ society.description }}</v-card-subtitle>
              <v-card-actions>
                <v-btn class="modern-btn" @click.stop="joinSociety(society.id)">
                  Join
                </v-btn>
              </v-card-actions>
            </v-card>
          </v-col>
        </v-row>

        <v-progress-linear v-if="loading" indeterminate color="light-blue darken-1"></v-progress-linear>
        <v-alert v-if="error" type="error" dense>{{ error }}</v-alert>
      </v-container>
    </v-main>
  </v-app>
</template>



<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import axios from 'axios';

const showDrawer = ref(true);
const router = useRouter();

interface Society {
  id: number;
  name: string;
  description: string;
  banner: string;
}

const societies = ref<Society[]>([]);
const loading = ref(false);
const error = ref('');

const fetchSocieties = async () => {
  loading.value = true;
  try {
    const response = await axios.get('/api/societies');
    societies.value = response.data;
  } catch (err) {
    error.value = 'Failed to load societies.';
  } finally {
    loading.value = false;
  }
};

const joinSociety = (id: number) => {
  alert(`Joined society with ID: ${id}`);
};

const goToAddSociety = () => {
  router.push('/add-society');
};

const openSociety = (id: number) => {
  router.push(`/societies/${id}`);
};

onMounted(() => {
  fetchSocieties();
});
</script>

<style scoped>
.content-container {
  margin-top: 64px;
  background: white;
  min-height: 100vh;
}

.modern-btn {
  background: linear-gradient(135deg, #007BFF, #00C6FF);
  color: white;
  font-size: 16px;
  font-weight: bold;
  text-transform: uppercase;
  padding: 12px 24px;
  border-radius: 30px;
  transition: all 0.3s ease-in-out;
  box-shadow: 0 4px 8px rgba(0, 123, 255, 0.3);
  display: flex;
  align-items: center;
  justify-content: center;
}

.modern-btn:hover {
  transform: scale(1.05);
}

.modern-btn:active {
  transform: scale(0.97);
}

.society-card {
  transition: 0.3s;
  border-radius: 10px;
  box-shadow: 0px 2px 10px rgba(0, 0, 0, 0.1);
  cursor: pointer;
}

.society-banner {
  height: 200px;
  object-fit: cover;
}
</style>
