<script lang="ts" setup>
import PostCard from "./PostCard.vue";
import { Post } from "../../apiClient";
import { onMounted, ref, computed } from "vue";
import axios from "axios";
import { useWindowScroll } from "@vueuse/core";

const { y } = useWindowScroll();
const posts = ref<Post[]>([]);
const numberOfPostsDisplayed = ref(10);

const displayedPosts = computed(() => {
	return posts.value.slice(0, numberOfPostsDisplayed.value);
});

function displayMorePosts() {
	if (numberOfPostsDisplayed.value + 10 < posts.value.length) {
		numberOfPostsDisplayed.value += 10;
	} else {
		numberOfPostsDisplayed.value = posts.value.length;
	}
}

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

function backToTop() {
	window.scrollTo({ top: 0, behavior: "smooth" });
}
</script>

<template>
	<div v-if="posts.length > 0" class="post-container">
		<post-card
			v-for="post in displayedPosts"
			:key="post.id"
			:post="post"
			class="post-item"
		></post-card>
		<v-btn
			v-if="numberOfPostsDisplayed !== posts.length"
			@click="displayMorePosts"
			class="ma-6"
			>Load more</v-btn
		>
		<p v-else>No more posts to display. Please try again later.</p>
	</div>
	<div v-else class="loading-container">
		<v-progress-circular indeterminate :size="52"></v-progress-circular>
	</div>
	<Transition name="fade">
		<v-btn
			v-if="y > 500"
			@click="backToTop"
			class="backToTopBtn"
			icon="mdi-arrow-up-drop-circle-outline"
		></v-btn>
	</Transition>
</template>

<style scoped>
.post-container {
	margin-top: 40px;
	display: flex;
	flex-direction: column;
	align-items: center;
	width: 100%;
	height: 100%;
	overflow-y: scroll;
	padding-right: 5px;
	margin-bottom: 15px;
}

.loading-container {
	display: flex;
	align-items: center;
	justify-content: center;
	min-height: 100vh;
}

.backToTopBtn {
	position: fixed;
	bottom: 5%;
	right: 5%;
}

.fade-enter-active,
.fade-leave-active {
	transition: opacity 0.3s ease;
}

.fade-enter-from,
.fade-leave-to {
	opacity: 0;
}
</style>
