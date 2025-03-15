<script lang="ts" setup>
import { ref, watch } from "vue";
import CommentsDialog from "./CommentsDialog.vue";
import AddCommentsDialog from "./AddCommentsDialog.vue";
import axios from "axios";
import { Post } from "../../apiClient";

const props = defineProps(["post"]);
const post: Post = props.post;

var topCommentLiked = ref(false);
var liked = ref(false);
var commentIconEnabled = ref(false);
var commentsDialogActive = ref(false);
var addCommentsDialogActive = ref(false);

watch(commentsDialogActive, () => {
	if (!commentsDialogActive.value) {
		commentIconEnabled.value = false;
	}
});

watch(liked, async () => {
	if (liked.value) {
		try {
			await axios.post("api/posts/add-upvote/" + post.id);
		} catch (error) {
			console.log(error);
		}
	} else if (!liked.value) {
		try {
			await axios.post("api/posts/remove-upvote/" + post.id);
		} catch (error) {
			console.log(error);
		}
	}
});
</script>

<template>
	<v-card elevation="12" class="ma-2" rounded="lg">
		<div class="d-flex align-center">
			<!-- Top Bar -->
			<div class="d-flex align-center ma-2">
				<v-avatar class="mr-3" size="32">
					<!-- <v-img
						:src="
							post.author?.imagePath?.length == 0
								? '/pp-fallback.png'
								: post.author?.imagePath
						"
						alt="Profile Picture"
					></v-img> -->
					<v-img :src="'/pp-fallback.png'" alt="Profile Picture"></v-img>
				</v-avatar>
				<p class="font-weight-bold">{{ post.author?.name }}</p>
			</div>
			<v-spacer color="black"></v-spacer>
			<p class="ma-2">{{ post.community?.name }}</p>
		</div>
		<div>
			<v-img alt="Post Image" :src="''" />
		</div>
		<!-- <v-divider v-if="post.author?.imagePath?.length == 0"></v-divider> -->
		<v-divider></v-divider>
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
			<p class="my-2 mr-8">{{ post.upvotes! + (liked ? 1 : 0) }} likes</p>
		</div>
		<v-divider></v-divider>
		<div class="d-flex description-wrapper">
			<!-- Username & Description -->
			<p class="ma-3 font-weight-bold">{{ post.author?.name }}</p>
			<v-spacer></v-spacer>
			<p class="ma-3">{{ post.content }}</p>
		</div>
		<v-divider></v-divider>
		<div>
			<!-- Comments -->
			<div>
				<p class="text-left ml-3 mb-2 font-weight-medium mt-2">
					{{
						post.comments && post.comments.length > 0
							? "Comments"
							: "No Comments"
					}}
				</p>
				<div class="d-flex" v-if="post.comments && post.comments.length > 0">
					<p class="mx-3 font-weight-bold">{{ post.comments?.at(0)?.author?.name }}</p>
					<p class="mx-3">
						{{ post.comments?.at(0)?.content }}
					</p>
					<!-- <v-icon
						:color="topCommentLiked ? 'red' : 'black'"
						:icon="topCommentLiked ? 'mdi-heart' : 'mdi-heart-outline'"
						class="mr-3"
						@click="topCommentLiked = !topCommentLiked"
					></v-icon> -->
				</div>
			</div>
			<div class="mt-1">
				<v-btn
					variant="plain"
					color="grey"
					@click="addCommentsDialogActive = true"
					density="comfortable"
					:ripple="false"
				>
					Add Comment
				</v-btn>
				<v-btn
					v-if="post.comments && post.comments.length > 0"
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
	<comments-dialog v-model:enabled="commentsDialogActive" :comments="post.comments" :topCommentLiked="topCommentLiked"/>
	<add-comments-dialog v-model:enabled="addCommentsDialogActive" />
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
