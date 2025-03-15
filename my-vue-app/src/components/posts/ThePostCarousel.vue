<script lang="ts" setup>
import PostCard from "./PostCard.vue";
import { Post } from "../../apiClient";
import { onMounted, ref, computed } from "vue";
import axios from "axios";

const posts = ref<Post[]>([]);
const numberOfPostsDisplayed = ref(6);

const displayedPosts = computed(() => {
	return posts.value.slice(0, numberOfPostsDisplayed.value);
});

onMounted(async () => {
	await fetchData();
});

async function fetchData() {
	try {
		const response = await axios.get("api/for-you/posts");
		posts.value = response.data;
	} catch (error) {
		console.error("Error:", error);
	}
}
</script>

<template>
	<div v-if="posts.length > 0" class="post-container" ref="el">
		<post-card
			v-for="post in displayedPosts"
			:key="post.id"
			:post="post"
		></post-card>
	</div>
	<div v-else class="loading-container">
		<v-progress-circular indeterminate :size="52"></v-progress-circular>
	</div>
</template>

<style scoped>
.post-container {
	display: flex;
	flex-direction: column;
	align-items: center;
	width: 100%;
	height: 100%;
	overflow-y: scroll;
	padding-right: 5px;
}

.loading-container {
	display: flex;
	align-items: center;
	justify-content: center;
	min-height: 100vh;
}
</style>
