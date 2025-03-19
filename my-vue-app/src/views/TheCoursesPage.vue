<template>
  <v-app>
    <v-navigation-drawer v-model="showDrawer" app color="white" light>
      <v-list>
        <v-list-item class="text-h6">My Courses</v-list-item>
        <v-divider></v-divider>
        <v-list-item v-for="course in courses" :key="course.id" @click="openCourse(course.id)">
          <v-list-item-content>
            <v-list-item-title>{{ course.name }}</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>

    <!-- Page Content -->
    <v-main class="content-container">
      <v-container>
        <!-- Add Course Button (Aligned with Cards) -->
        <v-row class="button-container">
          <v-col cols="12" md="6" lg="4">
            <v-btn class="modern-btn" @click="goToAddCourse">
              + Add Course
            </v-btn>
          </v-col>
        </v-row>

        <v-row>
          <v-col v-for="course in courses" :key="course.id" cols="12" md="6" lg="4">
            <v-card class="course-card" @click="openCourse(course.id)">
              <v-img :src="course.banner" class="course-banner"></v-img>
              <v-card-title>{{ course.name }}</v-card-title>
              <v-card-subtitle class="grey--text">{{ course.description }}</v-card-subtitle>
              <v-card-actions>
                <v-btn class="modern-btn" @click.stop="joinCourse(course.id)">
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

interface Course {
  id: number;
  name: string;
  description: string;
  banner: string;
}

const courses = ref<Course[]>([]);
const loading = ref(false);
const error = ref('');

const fetchCourses = async () => {
  loading.value = true;
  try {
    const response = await axios.get('/api/courses');
    courses.value = response.data;
  } catch (err) {
    error.value = 'Failed to load courses.';
  } finally {
    loading.value = false;
  }
};

const joinCourse = (id: number) => {
  alert(`Joined course with ID: ${id}`);
};

const goToAddCourse = () => {
  router.push('/add-course');
};

const openCourse = (id: number) => {
  router.push(`/courses/${id}`);
};

onMounted(() => {
  fetchCourses();
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
  display: flex;
  align-items: center;
  justify-content: center;
}

.modern-btn:hover {
  transform: scale(1.05);
}

.course-banner {
  height: 200px;
  object-fit: cover;
}
</style>
