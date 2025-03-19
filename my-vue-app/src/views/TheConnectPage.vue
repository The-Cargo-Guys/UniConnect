<template>
  <v-app>
    <v-main class="connect-container">
      <v-container>
        <!-- Page Title (Now Properly Centered & Aligned) -->
        <v-row justify="center">
          <v-col cols="12">
            <h1 class="page-title">Connect</h1>
          </v-col>
        </v-row>

        <!-- Search Section -->
        <v-row justify="center">
          <v-col cols="12" md="8" lg="6">
            <v-text-field
              v-model="searchQuery"
              label="Search Users"
              prepend-inner-icon="mdi-magnify"
              variant="outlined"
              class="search-bar"
              hide-details
              @keyup.enter="submitSearch"
            ></v-text-field>
          </v-col>
          <v-col cols="12" md="4" lg="2" class="d-flex justify-center">
            <v-btn class="modern-btn" @click="submitSearch">
              Search
            </v-btn>
          </v-col>
        </v-row>

        <!-- No Results Message -->
        <v-alert v-if="submittedQuery && filteredUsers.length === 0" type="info" class="no-results">
          No users found.
        </v-alert>

        <!-- Filtered Users List -->
        <v-row v-else-if="submittedQuery">
          <v-col v-for="user in paginatedFilteredUsers" :key="user.id" cols="12" md="6" lg="4">
            <v-card class="user-card" @click="goToUser(user.id)">
              <v-row align="center">
                <v-col cols="3" class="avatar-col">
                  <v-avatar size="60">
                    <v-img :src="user.avatar || 'https://via.placeholder.com/60'" alt="User Avatar"></v-img>
                  </v-avatar>
                </v-col>
                <v-col cols="9">
                  <v-card-title class="user-name">{{ user.name }}</v-card-title>
                  <v-card-subtitle class="grey--text">{{ user.email }}</v-card-subtitle>
                </v-col>
              </v-row>
            </v-card>
          </v-col>
        </v-row>

        <!-- Pagination -->
        <v-pagination
          v-if="totalPages > 1"
          v-model="currentPage"
          :length="totalPages"
          class="pagination"
        ></v-pagination>
      </v-container>
    </v-main>
  </v-app>
</template>


<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import axios from 'axios';
import { useRouter } from 'vue-router';

const searchQuery = ref('');
const submittedQuery = ref('');
const currentPage = ref(1);
const resultsPerPage = 9;
const router = useRouter();

interface User {
  id: number;
  name: string;
  email: string;
  avatar?: string;
}

const users = ref<User[]>([]);

// Fetch user data on mount
onMounted(async () => {
  try {
    const response = await axios.get('/api/users/search/');
    users.value = response.data;
  } catch (error) {
    console.error('Error fetching users:', error);
  }
});

// Filter users based on search
const filteredUsers = computed(() => {
  if (!submittedQuery.value) return [];
  return users.value.filter(user =>
    user.name.toLowerCase().includes(submittedQuery.value.toLowerCase())
  );
});

// Compute total pages
const totalPages = computed(() =>
  Math.ceil(filteredUsers.value.length / resultsPerPage)
);

// Paginate users
const paginatedFilteredUsers = computed(() => {
  const start = (currentPage.value - 1) * resultsPerPage;
  return filteredUsers.value.slice(start, start + resultsPerPage);
});

// Search function
const submitSearch = () => {
  submittedQuery.value = searchQuery.value;
  currentPage.value = 1; // Reset to first page on new search
};

// Navigate to user profile
const goToUser = (id: number) => {
  router.push({ name: 'ProfileDetails', params: { id } });
};
</script>


<style scoped>
/* Page Container */
.connect-container {
  background: white;
  min-height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
}

/* Page Title - Now Centered & Aligned */
.page-title {
  text-align: center;
  font-size: 2.5rem;
  font-weight: bold;
  margin-bottom: 30px;
}

/* Search Bar */
.search-bar {
  width: 100%;
  max-width: 100%;
  border-radius: 8px;
}

/* Modern Button */
.modern-btn {
  background: linear-gradient(135deg, #007BFF, #00C6FF);
  color: white;
  font-size: 18px;
  font-weight: bold;
  text-transform: uppercase;
  padding: 14px 28px;
  border-radius: 30px;
  transition: all 0.3s ease-in-out;
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100%;
  max-width: 180px;
}

.modern-btn:hover {
  transform: scale(1.05);
}

/* User List */
.user-card {
  cursor: pointer;
  transition: 0.3s;
  border-radius: 15px;
  padding: 15px;
  background: white;
  box-shadow: 0px 6px 15px rgba(0, 0, 0, 0.1);
}

.user-card:hover {
  transform: scale(1.03);
  box-shadow: 0px 8px 20px rgba(0, 0, 0, 0.2);
}

.user-name {
  font-size: 1.2rem;
  font-weight: bold;
}

/* Avatar */
.avatar-col {
  display: flex;
  justify-content: center;
  align-items: center;
}

/* No Results Message */
.no-results {
  text-align: center;
  font-size: 18px;
  font-weight: 500;
  color: #777;
  margin-top: 20px;
}

/* Pagination */
.pagination {
  display: flex;
  justify-content: center;
  margin-top: 30px;
}
</style>
