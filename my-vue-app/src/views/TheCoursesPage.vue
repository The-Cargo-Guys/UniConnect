<template>
  <div id="courses-page" class="container">
    <v-navigation-drawer v-model="showDrawer" color="secondary">
      <v-list-item title="My Societies"></v-list-item>
      <v-divider></v-divider>
      <v-list-item v-for="course in courses" :key="course.id" @click="openCourse(course.id)">{{ course.name }}</v-list-item>
    </v-navigation-drawer>
    <h1>Courses</h1>
    <v-btn class="drawerButton ma-5 pa-4" color="secondary" @click="closeOpenDrawer">
        <v-icon>mdi-menu</v-icon>
    </v-btn>
    <button class="add-course" @click="goToAddCourse">+ Add Course</button>
    <div class="courses-list">
      <div
        v-for="course in courses"
        :key="course.id"
        class="course-card"
        @click="openCourse(course.id)"
      >
        <img :src="course.banner" alt="Course Banner" class="course-banner" />
        <div class="course-info">
          <h2>{{ course.name }}</h2>
          <p>{{ course.description }}</p>
          <button @click.stop="joinCourse(course.id)" class="join-btn">
            Join
          </button>
        </div>
      </div>
    </div>
    <div v-if="loading" class="loading">Loading societies...</div>
    <div v-if="error" class="error">{{ error }}</div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import axios from 'axios';

export default defineComponent({
  name: 'CoursesPage',
  setup() {
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
        console.error('API Error:', err);
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

    const closeOpenDrawer = () => {
      showDrawer.value = !showDrawer.value;
    }

    onMounted(() => {
      fetchCourses();
    });

    return {
      showDrawer,
      closeOpenDrawer,
      courses,
      loading,
      error,
      joinCourse,
      goToAddCourse,
      openCourse
    };
  },
});
</script>

<style scoped>
.container {
  max-width: 1200px;
  margin: auto;
  padding: 2rem;
  text-align: center;
}

h1 {
  font-size: 2rem;
  margin-bottom: 1rem;
}

.add-course {
  background-color: #4a90e2;
  color: white;
  padding: 10px 20px;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  margin-bottom: 1rem;
}

.courses-list {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 20px;
}

.course-card {
  background: #d1caca;
  padding: 1rem;
  border-radius: 10px;
  text-align: left;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
  cursor: pointer;
  transition: transform 0.2s;
}

.course-card:hover {
  transform: scale(1.02);
}

.course-banner {
  width: 100%;
  height: 150px;
  object-fit: cover;
  border-radius: 8px;
}

.course-info {
  padding: 10px 0;
}

.join-btn {
  background-color: #ff4081;
  color: white;
  padding: 8px 16px;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}

.loading {
  margin-top: 1rem;
  color: #4a90e2;
}

.error {
  margin-top: 1rem;
  color: red;
}
</style>
