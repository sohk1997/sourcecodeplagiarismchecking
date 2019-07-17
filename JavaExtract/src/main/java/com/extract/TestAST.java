package com.extract;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.ObjectWriter;
import com.github.javaparser.StaticJavaParser;
import com.github.javaparser.ast.CompilationUnit;
import com.github.javaparser.ast.Node;
import com.github.javaparser.ast.expr.BinaryExpr;
import com.github.javaparser.ast.expr.IntegerLiteralExpr;
import com.github.javaparser.ast.expr.LiteralStringValueExpr;
import com.github.javaparser.serialization.JavaParserJsonSerializer;

import javax.json.Json;
import javax.json.stream.JsonGenerator;
import javax.json.stream.JsonGeneratorFactory;
import java.io.*;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class TestAST {

    public static void main(String[] args) {
        try {
            List<MethodClass> listMethods = new ArrayList<>();
            File folder = new File("BaseInput");
            File[] listOfFile = folder.listFiles();
            String traceFilename = "";
            for (File file : listOfFile) {
                if (!file.getName().contains(".java")) {
                    continue;
                }
                try {
                    traceFilename = file.getName();
                    try (BufferedReader isr = new BufferedReader(new InputStreamReader(new FileInputStream(file)))) {
                        StringBuilder code = new StringBuilder();
                        String line = null;
                        while ((line = isr.readLine()) != null) {
                            code.append(line);
                            code.append("\n");
                        }
                        CompilationUnit compilationUnit = StaticJavaParser.parse(code.toString());
                        NodeHandler handler = new NodeHandler();
                        handler.importNode(compilationUnit);
                        listMethods.addAll(handler.getMethodList());
                    }
                } catch (Exception ex) {
                    ex.printStackTrace();
                    System.out.println(traceFilename);
                }
            }

            try {
                ObjectWriter ow = new ObjectMapper().writer();
                String processedNode = ow.writeValueAsString(listMethods);
                try {
                    File myObj = new File("json.json");
                    myObj.createNewFile();
                    try (OutputStreamWriter osw = new OutputStreamWriter(new FileOutputStream("json.json"))) {
                        osw.write(processedNode);
                    }
                } catch (IOException e) {
                    System.out.println("An error occurred.");
                    e.printStackTrace();
                }
            } catch (Exception ex) {
                ex.printStackTrace();
            }
        } catch (Exception ex) {
            ex.printStackTrace();
        }
//        handler.print();
    }

    static String serialize(Node node, boolean prettyPrint) {
        Map<String, ?> config = new HashMap<>();
        if (prettyPrint) {
            config.put(JsonGenerator.PRETTY_PRINTING, null);
        }
        JsonGeneratorFactory generatorFactory = Json.createGeneratorFactory(config);
        JavaParserJsonSerializer serializer = new JavaParserJsonSerializer();
        StringWriter jsonWriter = new StringWriter();
        try (JsonGenerator generator = generatorFactory.createGenerator(jsonWriter)) {
            serializer.serialize(node, generator);
        }
        return jsonWriter.toString();
    }

    static void travel(Node node) {
//        if(node.getParentNode().isPresent()) {
//            System.out.println(node.getParentNode().get().getMetaModel().getTypeName() + " - " + node.getMetaModel().getTypeName());
//        }
//        else {
//            System.out.println("Root " + node.getMetaModel().getTypeName());
//        }
//        System.out.println(node);
//        System.out.println(node.getClass());
//        if(node.getClass().equals(com.github.javaparser.ast.expr.SimpleName.class))
//        {
//            SimpleName simpleName = (SimpleName)node;
//            System.out.println(simpleName.getIdentifier());
//        }
        System.out.print(node.getMetaModel().getTypeName());
        for (Node childNode : node.getChildNodes()) {
            System.out.print(" - " + childNode.getMetaModel().getTypeName());
        }

        if (com.github.javaparser.ast.expr.LiteralStringValueExpr.class.isAssignableFrom(node.getClass())) {
            LiteralStringValueExpr binaryExpr = (LiteralStringValueExpr) node;
            System.out.println("Value " + binaryExpr.getValue());
        }


        System.out.println();
        for (Node childNode : node.getChildNodes()) {
            travel(childNode);
        }

//        System.out.println("node value " + node.getBegin() + " end " + node.getEnd());
    }
}
