<script lang="ts">
import { defineComponent, ref } from "vue";
import { useRouter } from "vue-router";

export default defineComponent({
    name: "TheAddCoursePage",
    setup() {
        const router = useRouter();
        const course = ref({
            name: "",
            description: "",
            banner: ""
        });

        const submitCourse = () => {
            alert(`Course Created: ${course.value.name}`);
            router.push("/courses");
        };

        return {
            course,
            submitCourse
        };
    }
});
</script>

<template>
    <div class="add-course-container">
        <h1>Create a New Course</h1>
        <form @submit.prevent="submitCourse">
            <label for="name">Course Name:</label>
            <input type="text" id="name" v-model="course.name" required />

            <label for="description">Description:</label>
            <textarea id="description" v-model="course.description" required></textarea>

            <label for="banner">Banner Image URL:</label>
            <input type="text" id="banner" v-model="course.banner" required />

            <button type="submit">Create Course</button>
        </form>
    </div>
</template>

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
