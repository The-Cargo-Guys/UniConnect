<template>
    <div class="connect-page container">
      <h1>Connect</h1>
  
      <!-- Search Bar -->
      <v-text-field
        v-model="searchQuery"
        label="Search Users"
        prepend-inner-icon="mdi-magnify"
        variant="outlined"
        class="mb-4"
        hide-details
        @keyup.enter="submitSearch"
      ></v-text-field>
      
      <!-- Search Button -->
      <v-btn color="primary" class="mb-4" @click="submitSearch">Search</v-btn>
  
      <!-- No results message -->
      <div v-if="submittedQuery && filteredUsers.length === 0" class="no-results">
        No results found.
      </div>
  
      <!-- Filtered Users List: Only visible if a search has been submitted and there are results -->
      <v-list v-else-if="submittedQuery">
        <v-list-item
          v-for="user in paginatedFilteredUsers"
          :key="user.id"
          class="user-item"
        >
          <v-list-item-avatar>
            <v-img
              :src="user.avatar || 'https://via.placeholder.com/50'"
              alt="User Avatar"
              eager
            ></v-img>
          </v-list-item-avatar>
          <v-list-item-content>
            <v-list-item-title>{{ user.name }}</v-list-item-title>
            <v-list-item-subtitle>{{ user.email }}</v-list-item-subtitle>
          </v-list-item-content>
        </v-list-item>
      </v-list>
  
      <!-- Pagination Controls (only if there is more than one page) -->
      <v-pagination
        v-if="totalPages > 1"
        v-model="currentPage"
        :length="totalPages"
        class="my-4"
      ></v-pagination>
    </div>
  </template>
  
  <script lang="ts">
  import { defineComponent, ref, computed, onMounted } from 'vue';
  import axios from 'axios';
  
  export default defineComponent({
    name: 'ConnectPage',
    setup() {
      const searchQuery = ref('');
      const submittedQuery = ref('');
      const currentPage = ref(1);
      const resultsPerPage = 10;
  
      interface User {
        id: number;
        name: string;
        email: string;
        avatar?: string;
      }
      const users = ref<User[]>([]);
  
      // Fetch user data from an API on mount
      onMounted(async () => {
        try {
          const response = await axios.get('/api/users'); // Adjust endpoint as needed
          users.value = response.data;
        } catch (error) {
          console.error('Error fetching users:', error);
        }
      });
  
      // Filter users based on the submitted query only
      const filteredUsers = computed(() => {
        if (!submittedQuery.value) return [];
        const query = submittedQuery.value.toLowerCase();
        return users.value.filter((user: User) =>
          user.name.toLowerCase().includes(query)
        );
      });
  
      // Compute total pages
      const totalPages = computed(() =>
        Math.ceil(filteredUsers.value.length / resultsPerPage)
      );
  
      // Paginate the filtered users
      const paginatedFilteredUsers = computed(() => {
        const start = (currentPage.value - 1) * resultsPerPage;
        return filteredUsers.value.slice(start, start + resultsPerPage);
      });
  
      // Update submittedQuery when the user presses Enter or clicks Search
      const submitSearch = () => {
        submittedQuery.value = searchQuery.value;
        currentPage.value = 1; // Reset to first page on new search
      };
  
      return {
        searchQuery,
        submittedQuery,
        users,
        filteredUsers,
        paginatedFilteredUsers,
        currentPage,
        totalPages,
        submitSearch,
      };
    },
  });
  </script>
  
  <style scoped>
  .connect-page.container {
    max-width: 800px;
    margin: 0 auto;
    padding: 2rem;
  }
  
  .user-item {
    margin-bottom: 0.5rem;
  }
  
  .no-results {
    font-size: 16px;
    font-weight: 500;
    color: #777;
    margin-bottom: 1rem;
  }
  </style>
  