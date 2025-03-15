<script lang="ts">
import { defineComponent, ref } from "vue";
import { useRouter } from "vue-router";
  
export default defineComponent({
  name: "CoursesPage",
  setup() {
    const router = useRouter();
    const courses = ref([
      {
        id: 1,
        name: "Tech Enthusiasts",
        description: "A course for tech lovers to discuss and share knowledge.",
        banner: "https://via.placeholder.com/600x200",
      },
      {
        id: 2,
        name: "Gaming Club",
        description: "Join us for weekly gaming sessions and tournaments!",
        banner: "./images/UniConnect.png",
      },
    ]);

    const joinCourse = (id: number) => {
      alert(`Joined course with ID: ${id}`);
    };

    const goToAddCourse = () => {
      router.push("/add-course");
    };

    // Navigate to Course Details page using the route name "CourseDetails"
    const openCourse = (id: number) => {
      router.push({ name: "CoursesDetails", params: { id } });
    };

    return {
      courses,
      joinCourse,
      goToAddCourse,
      openCourse,
    };
  },
});
</script>

<template>
  <div id="courses-page" class="container">
    <h1>Courses</h1>
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
          <!-- The join button stops propagation so it doesn't trigger navigation -->
          <button @click.stop="joinCourse(course.id)" class="join-btn">
            Join
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

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
</style>
