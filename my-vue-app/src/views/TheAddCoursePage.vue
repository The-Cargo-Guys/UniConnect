<template>
    <div class="add-course-container">
        <h1>Create a New Course</h1>
        <form @submit.prevent="submitCourse">
            <label for="name">Course Name:</label>
            <input type="text" id="name" v-model="name" required />

            <label for="description">Description:</label>
            <textarea id="description" v-model="description" required></textarea>

            <label for="banner">Banner Image URL:</label>
            <input type="text" id="banner" v-model="imagePathBanner" required />

            <button type="submit">Create Course</button>
        </form>
    </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { useRouter } from "vue-router";
import axios from "axios";
import { Course } from "../apiClient";

export default defineComponent({
    name: "TheAddCoursePage",
    setup() {
        const router = useRouter();
        const name = ref("");
        const description = ref("");
        const imagePathBanner = ref("");

        const submitCourse = async() => {
            try {
                const course = new Course({
                    name: name.value,
                    description: description.value,
                    imagePathBanner: imagePathBanner.value,
                    });
    
                const response = await axios.post("api/courses", course);
                alert(`Course Created: ${response.data.name}`);
                router.push("/courses");

            }
            catch (error) {
                console.error("Error creating course:", error);
                alert("Failed to create course. Please check the server logs for details.");
            }
        };

        return {
            submitCourse,
            name, description, imagePathBanner
        };
    }
});
</script>
<style scoped>
.add-course-container {
    max-width: 600px;
    margin: auto;
    padding: 2rem;
    text-align: center;
    background: #fff;
    border-radius: 8px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
}

form {
    display: flex;
    flex-direction: column;
}

label {
    margin: 10px 0 5px;
    font-weight: bold;
}

input, textarea {
    padding: 10px;
    width: 100%;
    border: 1px solid #ccc;
    border-radius: 5px;
}

button {
    background-color: #4a90e2;
    color: white;
    padding: 10px;
    border: none;
    border-radius: 5px;
    margin-top: 10px;
    cursor: pointer;
}
</style>
