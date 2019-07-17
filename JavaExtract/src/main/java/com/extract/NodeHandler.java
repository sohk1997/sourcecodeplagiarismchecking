package com.extract;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.ObjectWriter;
import com.github.javaparser.ast.Node;
import com.github.javaparser.ast.expr.SimpleName;
import com.github.javaparser.ast.stmt.BlockStmt;
import com.github.javaparser.ast.stmt.SwitchEntry;

import java.lang.reflect.Method;
import java.util.ArrayList;
import java.util.List;
import java.util.Stack;

public class NodeHandler {

    private boolean isInMethod = false;

    private List<NodeMark> variableNode = null;
    private Stack<Node> blocks = null;
    private List<MethodClass> methodList = null;

    private Node node = null;
    private int variableCount = 0;
    private NodeSerialize processedTree = null;

    public NodeHandler() {
        blocks = new Stack<>();
        variableNode = new ArrayList<>();
        methodList = new ArrayList<>();
    }

    public NodeSerialize getProcessedTree() {
        return this.processedTree;
    }

    public List<MethodClass> getMethodList() {
        return methodList;
    }

    public String getProcessedTreeAsJSON() {
        try {
            ObjectWriter ow = new ObjectMapper().writer().withDefaultPrettyPrinter();
            String processedNode = ow.writeValueAsString(processedTree);
            return processedNode;
        } catch (Exception ex) {
            ex.printStackTrace();
            return null;
        }
    }

    public void importNode(Node node) {
        node = node.removeComment();
        processedTree = buildNodeSerialize(node);
        travel(node);

        this.node = node;
        synchronize(this.node, this.processedTree);
        for(MethodClass method : methodList)
        {
            method.setProcessedContent(method.getTree().getBaseNode().toString());
        }
    }

    public void synchronize(Node node, NodeSerialize serialize) {
        int index = 0;

        for (Node childNode : node.getChildNodes()) {
            while (index < serialize.countChild() && serialize.getChild(index).getBaseNode() != childNode) {
                serialize.removeChild(index);
            }
            if(index < serialize.countChild()) {
                synchronize(childNode, serialize.getChild(index));
            }
            index++;
        }
    }

    public NodeSerialize buildNodeSerialize(Node node)
    {
        NodeSerialize result = new NodeSerialize(node);
        Class nodeClass = getNodeClass(node);
        MethodClass method = null;
        if(!node.getBegin().isPresent() || !node.getEnd().isPresent())
        {
            return null;
        }
        result.setStartLine(node.getBegin().get().line);
        result.setEndLine(node.getEnd().get().line);
        if (nodeClass.equals(com.github.javaparser.ast.body.MethodDeclaration.class)) {
            method = new MethodClass(node.getBegin().get().line, node.getEnd().get().line, node.clone());
        }

        for (Node childNode : node.getChildNodes()) {
            NodeSerialize childNodeSerialize = buildNodeSerialize(childNode);
            if (childNodeSerialize != null) {
                result.addChild(childNodeSerialize);
            }
        }

        if (nodeClass.equals(com.github.javaparser.ast.body.MethodDeclaration.class)) {
            method.setTree(result);
            methodList.add(method);
        }

        return result;
    }

    //This will remove all unnecessary Variable Declaration
    public void travel(Node node) {
        Class nodeClass = getNodeClass(node);
        if (nodeClass.equals(com.github.javaparser.ast.body.MethodDeclaration.class)) {
            isInMethod = true;
        }
        if (isInMethod) {
            if (nodeClass.equals(com.github.javaparser.ast.stmt.BlockStmt.class) ||
                    nodeClass.equals(com.github.javaparser.ast.stmt.SwitchEntry.class)) {
                //block statement
                blocks.push(node);
            } else {
                if (nodeClass.equals(com.github.javaparser.ast.expr.SimpleName.class)) {
                    //Handle simple name
                    if (getNodeClass(node.getParentNode().get()).equals(com.github.javaparser.ast.body.VariableDeclarator.class)
                            || getNodeClass(node.getParentNode().get()).equals(com.github.javaparser.ast.body.Parameter.class)) {
                        if (!blocks.empty()) {
                            NodeMark mark = new NodeMark(node, blocks.peek());
                            variableNode.add(mark);
                        } else {
                            NodeMark mark = new NodeMark(node, null);
                            variableNode.add(mark);
                        }
                    } else {
                        if (getNodeClass(node.getParentNode().get()).equals(com.github.javaparser.ast.expr.NameExpr.class)) {
                            SimpleName simpleName = (SimpleName) node;
                            boolean isHave = false;
                            for (int index = variableNode.size() - 1; index >= 0; index--) {
                                NodeMark mark = variableNode.get(index);
                                if (mark.getSimpleName().getIdentifier().equals(simpleName.getIdentifier())) {
                                    mark.addReferences(node);
                                    isHave = true;
                                    break;
                                }
                            }
                            if (!isHave) {
                                //this is class property
                                NodeMark mark = new NodeMark(node, null);
                                variableNode.add(mark);
                            }
                        }
                    }
                }
            }
        }
        for (Node childNode : node.getChildNodes()) {
          travel(childNode);
        }
        if (nodeClass.equals(com.github.javaparser.ast.stmt.BlockStmt.class) ||
                nodeClass.equals(com.github.javaparser.ast.stmt.SwitchEntry.class)) {
            //block statement
            blocks.pop();
            BlockStmt block = null;
            SwitchEntry switchStm = null;
            if (nodeClass.equals(com.github.javaparser.ast.stmt.BlockStmt.class)) {
                block = (BlockStmt) node;
            }
            if (nodeClass.equals(com.github.javaparser.ast.stmt.SwitchEntry.class)) {
                switchStm = (SwitchEntry) node;
            }
            int removeIndex = -1;
            if (!variableNode.isEmpty()) {
                for (int index = variableNode.size() - 1; index >= 0; index--) {
                    NodeMark mark = variableNode.get(index);
                    if (mark.getBlockNode() != null && mark.getBlockNode().equals(node)) {
                        removeIndex = index;
                        if (!mark.isUsed()) {
                            Node declaration = getVariableDeclaration(mark.getNode(), node);
                            if(declaration != null) {
                                if (block != null) {
                                    block.remove(declaration);
                                }
                                if (switchStm != null) {
                                    switchStm.remove(declaration);
                                }
                            }
                        } else {
                            mark.renameAllReferences(getNextVariableName());
                        }
                    } else {
                        break;
                    }

                }
                if (removeIndex != -1) {
                    variableNode = variableNode.subList(0, removeIndex);
                }
            }
        } else {
            if (nodeClass.equals(com.github.javaparser.ast.body.MethodDeclaration.class)) {
                isInMethod = false;
                for (int index = variableNode.size() - 1; index >= 0; index--) {
                    NodeMark mark = variableNode.get(index);
                    mark.renameAllReferences(getNextVariableName());
                }
                variableNode.clear();
                resetNumber();
            }
        }
    }

    private Class getNodeClass(Node node) {
        return node.getClass();
    }

    //Get unnecessary Variable Declaration
    private Node getVariableDeclaration(Node node, Node blockNode) {
        while (node.getParentNode().isPresent() && !node.getParentNode().get().equals(blockNode)) {
            node = node.getParentNode().get();
        }
        if(!node.getParentNode().isPresent())
        {
            return null;
        }
        return node;
    }

    private String getNextVariableName() {
        variableCount++;
        return "VARIABLE_" + variableCount;
    }

    private void resetNumber() {
        variableCount = 0;
    }
}
