<script lang="ts" setup>
import { ref, watch } from "vue";
import CommentsDialog from "./CommentsDialog.vue";

const props = defineProps(["username", "description", "likes", "imagePath"]);

var topCommentLiked = ref(false);
var liked = ref(false);
var commentIconEnabled = ref(false);
var commentsDialogActive = ref(false);

watch(commentsDialogActive, () => {
	if (!commentsDialogActive.value) {
		commentIconEnabled.value = false;
	}
});
</script>

<template>
	<v-card elevation="12" class="ma-2" rounded="lg">
		<div class="d-flex align-center">
			<!-- Top Bar -->
			<div class="d-flex align-center ma-2">
				<v-avatar class="mr-3" size="32">
					<v-img
						src="https://cdn.vuetifyjs.com/images/john.jpg"
						alt="Profile Picture"
					></v-img>
				</v-avatar>
				<p class="font-weight-medium">{{ props.username }}</p>
			</div>
			<v-spacer color="black"></v-spacer>
			<p class="ma-2">Degree/Society</p>
		</div>
		<div>
			<v-img
				alt="Post Image"
				:src="props.imagePath"
			/>
		</div>
		<div class="d-flex">
			<!-- Likes Bar -->
			<div class="my-2 ml-6">
				<v-icon
					:icon="liked ? 'mdi-heart' : 'mdi-heart-outline'"
					class="mx-1"
					@click="liked = !liked"
					:color="liked ? 'red' : 'black'"
				></v-icon>
				<v-icon
					:icon="commentIconEnabled ? 'mdi-comment' : 'mdi-comment-outline'"
					class="mx-1"
					@click="
						commentsDialogActive = true;
						commentIconEnabled = true;
					"
				></v-icon>
				<v-icon icon="mdi-bookmark-outline" class="mx-1"></v-icon>
			</div>
			<v-spacer class="my-4"></v-spacer>
			<p class="my-2 mr-8">{{ props.likes }} likes</p>
		</div>
		<v-divider></v-divider>
		<div class="d-flex description-wrapper">
			<!-- Username & Description -->
			<p class="ma-3 font-weight-medium">@{{ props.username }}</p>
			<v-spacer></v-spacer>
			<p class="ma-3">{{ props.description }}</p>
		</div>
		<v-divider></v-divider>
		<div>
			<!-- Comments -->
			<div>
				<p class="text-left ml-6 mb-2 font-weight-medium mt-2">Comments</p>
				<div class="d-flex">
					<p class="mx-3">@Joe</p>
					<v-spacer></v-spacer>
					<p class="mx-3">
						This is the top comment. Lorem ipsum dolor sit amet.
					</p>
					<v-icon
						:color="topCommentLiked ? 'red' : 'black'"
						:icon="topCommentLiked ? 'mdi-heart' : 'mdi-heart-outline'"
						class="mr-3"
						@click="topCommentLiked = !topCommentLiked"
					></v-icon>
				</div>
			</div>
			<div>
				<v-btn
					variant="plain"
					color="grey"
					@click="commentsDialogActive = true"
					density="comfortable"
					:ripple="false"
				>
					View More Comments
				</v-btn>
			</div>
		</div>
	</v-card>
	<comments-dialog v-model:enabled="commentsDialogActive"></comments-dialog>
</template>

<style scoped>
.v-card {
	width: 500px;
	max-height: 900px;
}

.description-wrapper {
	max-height: 100px;
}
</style>
