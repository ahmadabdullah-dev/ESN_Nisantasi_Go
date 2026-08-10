import { useMutation, useQuery } from "@tanstack/react-query";
import agent from "../api/agent";
import type { AddPlanDto, PlanDto } from "../types/plan";
import type { PaginatedList, PaginationParams } from "../types/common";

export const usePlan = (pagination?: PaginationParams) => {
  const getPlansAsync = useQuery({
    queryKey: ["plans", pagination?.page, pagination?.pageSize],
    queryFn: async () =>
    await agent.get<PaginatedList<PlanDto>>("/Plan/paged", { params: pagination }).then((res) => res.data),
    enabled: !!pagination,
    staleTime: 5 * 60 * 1000,
  });

  const addPlanAsync = useMutation({
    mutationFn: async (creds: AddPlanDto) => {
      const response = await agent.post("/Plan/add", creds);
      return response.data;
    },
  });

  const joinPlan = useMutation({
    mutationFn: async (id: string) => {
      const response = await agent.post("/Plan/join", id);
      return response.data;
    },
  });

  const leavePlan = useMutation({
    mutationFn: async (id: string) => {
      const response = await agent.post("/Plan/leave", id);
      return response.data;
    },
  });

  return {
    getPlansAsync,
    addPlanAsync,
    joinPlan,
    leavePlan,
  };
};

export function usePlanById(id: string) {
  return useQuery({
    queryKey: ["plans", id],
    queryFn: async () => agent.get<PlanDto>("/plan", { params: { planId: id } }).then((res) => res.data),
    staleTime: 5 * 60 * 1000,
    enabled: !!id,
  });
}
