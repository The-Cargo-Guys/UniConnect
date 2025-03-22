<script setup lang="ts">
import { Comment } from "../../apiClient";
import { ref, computed, watch } from "vue";

const props = defineProps(["enabled", "comments", "topCommentLiked"]);
var comments: Comment[] = props.comments;
var commentLiked = ref(new Array(comments.length).fill(false));

const emit = defineEmits(["update:enabled"]);

function toggleLike(index: number) {
	commentLiked.value[index] = !commentLiked.value[index];
}
</script>

<template>
	<v-dialog
		:model-value="props.enabled"
		@update:model-value="emit('update:enabled', $event)"
		max-width="500"
	>
		<template v-slot:default="{ isActive }">
			<v-card>
				<v-card-text>
					<div v-for="(comment, index) in comments" class="ma-6" :key="index">
						<div class="d-flex justify-space-between">
							<div class="d-flex align-center">
								<v-avatar class="mb-1">
									<v-img
										:src="comment.author?.imagePath"
										alt="Profile Picture"
										
									></v-img>
								</v-avatar>
								<p class="font-weight-bold ml-2">{{ comment.author?.name }}</p>
							</div>
							<v-icon
								:color="commentLiked[index] ? 'red' : 'black'"
								:icon="commentLiked[index] ? 'mdi-heart' : 'mdi-heart-outline'"
								@click="toggleLike(index)"
							></v-icon>
						</div>
						<p>{{ comment.content }}</p>
					</div>
				</v-card-text>
				<v-card-actions>
					<v-spacer></v-spacer>
					<v-btn text="Close" @click="isActive.value = false"></v-btn>
				</v-card-actions>
			</v-card>
		</template>
	</v-dialog>
</template>
