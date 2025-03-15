<template>
    <div class="add-society-container">
        <h1>Create a New Society</h1>
        <form @submit.prevent="submitSociety">
            <label for="name">Society Name:</label>
            <input type="text" id="name" v-model="society.name" required />

            <label for="description">Description:</label>
            <textarea id="description" v-model="society.description" required></textarea>

            <label for="banner">Banner Image URL:</label>
            <input type="text" id="banner" v-model="society.banner" required />

            <button type="submit">Create Society</button>
        </form>
    </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { useRouter } from "vue-router";
import axios from "axios";
import { CreateSocietyModel } from "../apiClient";

export default defineComponent({
    name: "TheAddSocietyPage",
    setup() {
        const router = useRouter();
        const society = ref({
            name: "",
            description: "",
            banner: ""
        });

        const submitSociety = async () => {
            try {
                const response = await axios.post("/api/societies", society.value);
                alert(`Society Created: ${response.data.name}`);
                router.push("/societies");
            } catch (error) {
                console.error("Error creating society:", error);
                alert("Failed to create society. Please check the server logs for details.");
            }
        };

        return {
            society,
            submitSociety
        };
    }
});
</script>

<style scoped>
.add-society-container {
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
