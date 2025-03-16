<script setup>
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import axios from 'axios';

const route = useRoute();
const userId = route.params.userId;
const user = ref(null);
const error = ref(null);
const loading = ref(true);

const fetchUserDetails = async () => {
    try {
        const response = await axios.get(`/api/users/${userId}`);
        user.value = response.data;
    } catch (err) {
        error.value = err.response?.data?.message || "Failed to load user details.";
    } finally {
        loading.value = false;
    }
};

onMounted(fetchUserDetails);
</script>

<template>
    <div class="profile-container">
        <h1>User Profile</h1>
        <div v-if="loading">Loading...</div>
        <div v-else-if="error" class="error">{{ error }}</div>
        <div v-else class="profile-card">
            <img :src="user.ImagePath || '/default-avatar.png'" alt="Profile Picture" class="profile-image" />
            <h2>{{ user.Name }}</h2>
            <p>Email: {{ user.Email }}</p>
            <p>Phone: {{ user.PhoneNumber }}</p>
            <p>Bio: {{ user.Bio || 'No bio available' }}</p>
            <p>University: {{ user.University }}</p>
            <p>Degree: {{ user.Degree }}</p>
            <div v-if="user.Tags && user.Tags.length">
                <h3>Tags:</h3>
                <ul>
                    <li v-for="tag in user.Tags" :key="tag.Id">{{ tag.Value }}</li>
                </ul>
            </div>
            <p v-if="user.IsAdmin" class="admin-badge">Administrator</p>
        </div>
    </div>
</template>

<style scoped>
.profile-container {
    max-width: 600px;
    margin: 20px auto;
    padding: 20px;
    border-radius: 10px;
    background: #f4f4f4;
    box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
}

.profile-card {
    text-align: center;
}

.profile-image {
    width: 120px;
    height: 120px;
    border-radius: 50%;
    object-fit: cover;
    margin-bottom: 10px;
}

.admin-badge {
    color: white;
    background: red;
    padding: 5px 10px;
    border-radius: 5px;
    display: inline-block;
    margin-top: 10px;
}

.error {
    color: red;
    font-weight: bold;
}
</style>
